using HugoModels;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using WordpressWXR12;
using YamlDotNet.Serialization;

namespace ConverterLibrary
{
    public class WordpressToHugoConverter
    {
        private readonly ILogger<WordpressToHugoConverter> _logger;
        private readonly IMapper _mapper;
        private readonly Serializer _yamlSerializer;
        private readonly IWordpressWXRParser _parser;

        public WordpressToHugoConverter(
            ILogger<WordpressToHugoConverter> logger, 
            IMapper mapper, 
            Serializer yamlSerializer, 
            IWordpressWXRParser parser)
        {
            _logger = logger;
            _mapper = mapper;
            _yamlSerializer = yamlSerializer;
            _parser = parser;
        }

        public void Convert(ConverterOptions options)
        {
            Directory.CreateDirectory(options.OutputDirectory);
            _logger.LogInformation($"Output will be written to: '{options.OutputDirectory}'.");

            if (ValidateOptions(options))
            {
                _logger.LogInformation($"Start processing '{options.InputFile}'...");
                var content = _parser.LoadFromFile(options.InputFile);

                WriteConfig(content, options);
                WritePosts(content, options);

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
                }));

            Directory.CreateDirectory(Path.Combine(options.OutputDirectory, "posts"));
            foreach (var hugoPost in hugoPosts)
            {
                string directory = Path.GetDirectoryName(hugoPost.Filename);
                string fileName;
                if (string.IsNullOrEmpty(directory))
                {
                    fileName = Path.Combine(options.OutputDirectory, "posts", hugoPost.Filename);
                }
                else
                {
                    Directory.CreateDirectory(Path.Combine(options.OutputDirectory, directory));
                    fileName = Path.Combine(options.OutputDirectory, hugoPost.Filename);
                }

                var yaml = _yamlSerializer.Serialize(hugoPost.Metadata);

                StringBuilder hugoYaml = new StringBuilder();
                hugoYaml.AppendLine("---");
                hugoYaml.AppendLine(yaml);
                hugoYaml.AppendLine("---");
                hugoYaml.AppendLine(hugoPost.Content);

                File.WriteAllText(fileName, hugoYaml.ToString(), Encoding.UTF8);
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
