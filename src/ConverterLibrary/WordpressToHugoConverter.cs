using System;
using System.Collections.Generic;
using System.Globalization;
using HugoModels;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using ConverterLibrary.Extensions;
using ConverterLibrary.Replacers.ImageReplacer;
using WordpressWXR12;
using YamlDotNet.Serialization;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private const string HugoStaticDirectoryName = "static";
        private const string HugoContentDirectoryName = "content";

        private readonly ILogger<WordpressToHugoConverter> _logger;
        private readonly IMapper _mapper;
        private readonly Serializer _yamlSerializer;
        private readonly IWordpressWXRParser _parser;
        private readonly ImageReplacer _imageReplacer;

        public WordpressToHugoConverter(
            ILogger<WordpressToHugoConverter> logger, 
            IMapper mapper, 
            Serializer yamlSerializer, 
            IWordpressWXRParser parser,
            ImageReplacer imageReplacer)
        {
            _logger = logger;
            _mapper = mapper;
            _yamlSerializer = yamlSerializer;
            _parser = parser;
            _imageReplacer = imageReplacer;
        }

        public void Convert(ConverterOptions options)
        {
            Directory.CreateDirectory(options.OutputDirectory);
            _logger.LogInformation($"Output will be written to: '{options.OutputDirectory}'.");
            if (options.PageResources)
            {
                _logger.LogInformation("Hugo page resources will be created.");
            }

            if (!string.IsNullOrEmpty(options.ImageShortCode))
            {
                if (!options.PageResources)
                {
                    _logger.LogError("Shortcode for images can only be used when generating Page Resources as well.");
                    return;
                }

                _logger.LogInformation($"Using shortcode for images: {options.ImageShortCode}");
            }

            if (ValidateOptions(options))
            {
                _logger.LogInformation($"Start processing '{options.InputFile}'...");
                var content = _parser.LoadFromFile(options.InputFile);

                WriteConfig(content, options);
                WritePosts(content, options);
                WriteComments(content, options);

                _logger.LogInformation("Done.");
            }
        }

        private void WritePosts(RSS content, ConverterOptions options)
        {
            var posts = content.Channel.Items.Where(item => item.PostType == "post" || item.PostType == "page").ToList();
            _logger.LogInformation($"Found {posts.Count} posts/pages.");

            var attachments = content.Channel.Items.Where(item => item.PostType == "attachment").ToDictionary(key => key.PostId);

            var hugoPosts = posts.Select(post => _mapper.Map<Post>(post, opts =>
                {
                    opts.Items[ConverterLibraryAutoMapperProfile.ItemNameAttachments] = attachments;
                    opts.Items[ConverterLibraryAutoMapperProfile.ItemNameSiteUrl] = content.Channel.Link;
                    opts.Items[ConverterLibraryAutoMapperProfile.ItemNamePageResources] = options.PageResources;
                }));

            foreach (var hugoPost in hugoPosts)
            {
                string outputDirectory = hugoPost.Metadata.Type == "page"
                    ? options.OutputDirectory
                    : Path.Combine(options.OutputDirectory, HugoContentDirectoryName);
                string postFileName = Path.Combine(outputDirectory, hugoPost.Filename);
                string postFullDirectory = Path.GetDirectoryName(postFileName);
                Directory.CreateDirectory(postFullDirectory);

                string imageBaseUrl = options.PageResources ? null : GetImageBaseUrl(hugoPost, "/uploads");
                var replacedImages = _imageReplacer.Replace(hugoPost, content.Channel.Link, imageBaseUrl, options.ImageShortCode);

                CopyReplacedImagesToOutputDirectory(options, replacedImages, postFullDirectory);

                var yaml = _yamlSerializer.Serialize(hugoPost.Metadata);

                StringBuilder hugoYaml = new StringBuilder();
                hugoYaml.AppendLine("---");
                hugoYaml.AppendLine(yaml);
                hugoYaml.AppendLine("---");
                hugoYaml.AppendLine(hugoPost.Content);

                File.WriteAllText(postFileName, hugoYaml.ToString(), Encoding.UTF8);
                _logger.LogInformation($"Written '{postFileName}'.");
            }
        }

        private void CopyReplacedImagesToOutputDirectory(ConverterOptions options, IEnumerable<ImageReplacerResult> replacedImages, string postFullDirectory)
        {
            if (options.UploadDirectories != null && options.UploadDirectories.Any())
            {
                foreach (var replacedImage in replacedImages)
                {
                    bool imageFound = false;
                    List<string> checkedPaths = new List<string>();

                    foreach (var uploadDirectory in options.UploadDirectories)
                    {
                        string originalImage = Path.Combine(uploadDirectory, replacedImage.OriginalRelativeUrl.UrlToPath().RemoveFirstBackslash());
                        string outputDirectory = Path.Combine(options.OutputDirectory, options.PageResources ? HugoContentDirectoryName : HugoStaticDirectoryName);
                        string newImageLocation = Path.Combine(options.PageResources ? postFullDirectory : outputDirectory, replacedImage.NewRelativeUrl.UrlToPath().RemoveFirstBackslash());

                        if (File.Exists(originalImage))
                        {
                            string directoryName = Path.GetDirectoryName(newImageLocation);
                            if (!Directory.Exists(directoryName))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(newImageLocation));
                            }

                            File.Copy(originalImage, newImageLocation, true);

                            _logger.LogInformation($"Copied '{originalImage}' to '{newImageLocation}'.");
                            imageFound = true;
                            break;
                        }
                        else
                        {
                            checkedPaths.Add(originalImage);
                        }
                    }

                    if (!imageFound)
                    {
                        _logger.LogWarning($"Could not find content '{string.Join(',', checkedPaths)}'.");
                    }
                }
            }
        }

        private string GetImageBaseUrl(Post hugoPost, string staticUploads)
        {
            string imageBaseUrl = staticUploads;

            if (DateTime.TryParseExact(
                hugoPost.Metadata.Date,
                ConverterLibraryAutoMapperProfile.HugoDateTimeFormat, 
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeLocal,
                out DateTime date))
            {
                imageBaseUrl = $"{staticUploads}/{date.Year:0000}/{date.Month:00}/";
            }

            return imageBaseUrl;
        }

        private void WriteComments(RSS content, ConverterOptions options)
        {
            var posts = content.Channel.Items.Where(item => item.PostType == "post" || item.PostType == "page").ToList();
            var staticmanComments = posts.SelectMany(post => _mapper.Map<IEnumerable<HugoModels.Comment>>(post));

            var commentsDirectory = Directory.CreateDirectory(Path.Combine(options.OutputDirectory, "data\\comments"));
            foreach (var comment in staticmanComments)
            {
                string directory = Path.GetDirectoryName(comment.FileName);
                string fileName;
                if (string.IsNullOrEmpty(directory))
                {
                    fileName = Path.Combine(commentsDirectory.FullName, comment.FileName);
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(commentsDirectory.FullName, directory));
                    fileName = Path.Combine(commentsDirectory.FullName, comment.FileName);
                }

                var yaml = _yamlSerializer.Serialize(comment.Metadata);

                File.WriteAllText(fileName, yaml, Encoding.UTF8);
                _logger.LogInformation($"Written '{fileName}'.");
            }
        }

        private void WriteConfig(RSS content, ConverterOptions options)
        {
            var config = new Config
            {
                Name = content.Channel.Title,
                Description = content.Channel.Description,
                Url = content.Channel.Link
            };

            string fileName = Path.Combine(options.OutputDirectory, "config.yaml");
            var yaml = _yamlSerializer.Serialize(config);

            File.WriteAllText(fileName, yaml, Encoding.UTF8);
            _logger.LogInformation($"Written '{fileName}'.");
        }

        private bool ValidateOptions(ConverterOptions options)
        {
            bool result = true;

            if (!File.Exists(options.InputFile))
            {
                _logger.LogError($"Input file '{options.InputFile}' not found.");
                result = false;
            }

            return result;
        }
    }
}
