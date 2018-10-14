using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConverterLibrary.Extensions;
using HugoModels;

namespace ConverterLibrary.Replacers.ImageReplacer
{
    public class ImageReplacer
    {
        /// <example>
        /// [![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]
        /// [![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg "test")]
        /// [![test](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)]
        /// </example>>
        private static readonly string _patternMarkdownImages = "\\!\\[(?<alt>.*?)\\]\\((?<url>.*?)(\"+(?<title>.*?)\"+)*\\)";
        private readonly Regex _regexMarkdownImages = new Regex(_patternMarkdownImages, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public IEnumerable<ImageReplacerResult> Replace(Post hugoPost, string siteUrl, string imageBaseUrl, string imageShortCode = null)
        {
            List<ImageReplacerResult> replacedImageResults = new List<ImageReplacerResult>();

            ReplaceImageLinksInContent(siteUrl, imageBaseUrl, hugoPost, replacedImageResults, imageShortCode);
            ReplaceLinksToImagesInContent(siteUrl, replacedImageResults, hugoPost);
            ReplaceImageLinksInResources(imageBaseUrl, hugoPost, replacedImageResults);

            return replacedImageResults;
        }

        private void ReplaceImageLinksInResources(string imageBaseUrl, Post hugoPost, List<ImageReplacerResult> replacedImageResults)
        {
            foreach (var resource in hugoPost.Metadata.Resources)
            {
                var originalRelativeUrl = resource.Src;
                var originalRelativeUrlSegments = originalRelativeUrl.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var originalImageName = originalRelativeUrlSegments.Last();
                var newRelativeUrl = imageBaseUrl.CombineUri(originalImageName);

                if (string.Compare(originalRelativeUrl, newRelativeUrl, StringComparison.InvariantCulture) != 0)
                {
                    resource.Src = newRelativeUrl;

                    replacedImageResults.Add(new ImageReplacerResult
                    {
                        OriginalRelativeUrl = originalRelativeUrl,
                        NewRelativeUrl = newRelativeUrl
                    });
                }
            }
        }

        private void ReplaceLinksToImagesInContent(string siteUrl, List<ImageReplacerResult> replacedImageResults, Post hugoPost)
        {
            string newContent = hugoPost.Content;

            foreach (var replacedImage in replacedImageResults)
            {
                newContent = ReplaceHyperlinksToImages(
                    newContent,
                    siteUrl.CombineUri(replacedImage.OriginalRelativeUrl),
                    replacedImage.NewRelativeUrl);
            }

            hugoPost.Content = newContent;
        }

        private void ReplaceImageLinksInContent(string siteUrl, string imageBaseUrl, Post hugoPost,
            List<ImageReplacerResult> replacedImageResults, string imageShortCode)
        {
            var resources = new List<Resource>();

            string replacedContent = _regexMarkdownImages.Replace(hugoPost.Content ?? string.Empty, m =>
            {
                string title = m.Groups["title"].Value;
                string alt = m.Groups["alt"].Value;
                string url = m.Groups["url"].Value;

                string newImgTag = GetNewImgTag(title, alt, url, siteUrl, imageBaseUrl, imageShortCode, out string originalRelativeUrl, 
                    out string newRelativeUrl);

                resources.Add(CreateResource(newRelativeUrl, title, alt));

                replacedImageResults.Add(new ImageReplacerResult
                {
                    OriginalRelativeUrl = originalRelativeUrl,
                    NewRelativeUrl = newRelativeUrl
                });

                return newImgTag;
            });

            hugoPost.Content = replacedContent;
            AddResourcesToPost(hugoPost, resources);
        }

        private void AddResourcesToPost(Post hugoPost, IEnumerable<Resource> resources)
        {
            if (hugoPost.Metadata == null)
            {
                hugoPost.Metadata = new PostMetadata();
            }

            var originalResources = hugoPost.Metadata.Resources ?? new List<Resource>();
            var newCombinedResources = originalResources.Concat(resources);

            hugoPost.Metadata.Resources = newCombinedResources;
        }

        private Resource CreateResource(string newRelativeUrl, string title, string alt)
        {
            return new Resource
            {
                Src = newRelativeUrl.Split('/', StringSplitOptions.RemoveEmptyEntries).Last(),
                Title = string.IsNullOrEmpty(title) ? string.IsNullOrEmpty(alt) ? null : alt : title
            };
        }

        private string GetNewImgTag(string title, string alt, string imageUrl, string siteUrl, string imageBaseUrl, string shortCode, out string originalRelativeUrl, out string newRelativeUrl)
        {
            string titleTag = string.IsNullOrEmpty(title) ? string.Empty : $" \"{title}\"";
            string newImgTag = $"![{alt}]({imageUrl}{titleTag})";
            originalRelativeUrl = imageUrl;
            newRelativeUrl = imageUrl;

            if (!string.IsNullOrEmpty(imageUrl) && !string.IsNullOrEmpty(siteUrl))
            {
                var imageUri = new Uri(imageUrl);
                var siteUri = new Uri(siteUrl);

                originalRelativeUrl = imageUri.AbsolutePath;

                if (siteUri.Host == imageUri.Host)
                {
                    string imageFilename = imageUri.Segments.Last().CleanUrl();
                    newRelativeUrl = imageBaseUrl.CombineUri(imageFilename);

                    if (string.IsNullOrEmpty(shortCode))
                    {
                        newImgTag = $"![{alt}]({newRelativeUrl}{titleTag})";
                    }
                    else
                    {
                        string formattedShortCode = string.Format(shortCode, imageFilename, title, alt);
                        newImgTag = string.Format("{{{{< {0} >}}}}", formattedShortCode);
                    }
                }
            }

            return newImgTag;
        }

        /// <example>
        /// [![](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg "test")](http://www.site.com/weblog/wp-content/uploads/2010/08/image.jpg)
        /// </example>
        /// <remarks>
        /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions#balancing_group_definition
        /// https://stackoverflow.com/a/7899205
        /// </remarks>
        private const string PatternImageLinks = @"
                \[                     # First '['
                    (?:                 
                    [^\[\]]            # Match all non-braces
                    |
                    (?<open> \[ )      # Match '[', and capture into 'open'
                    |
                    (?<-open> \] )     # Match ']', and delete the 'open' capture
                    )+
                    (?(open)(?!))      # Fails if 'open' stack isn't empty!

                \]                     # Last ']'
                \((?<url>.*?)\)        # match '(http://www.site.com/weblog)'
            ";
        private readonly Regex _regexImageLinks = new Regex(PatternImageLinks, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private string ReplaceHyperlinksToImages(string yaml, string orgLinkUrl, string newLinkUrl)
        {
            var replacedYaml = yaml;
            var orgLinkUri = new Uri(orgLinkUrl);

            var matches = _regexImageLinks.Matches(yaml);
            foreach (Match match in matches)
            {
                string url = match.Groups["url"].Value;

                if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri) &&
                    uri.Host == orgLinkUri.Host &&
                    uri.AbsolutePath == orgLinkUri.AbsolutePath)
                {
                    replacedYaml = replacedYaml.Replace(url, newLinkUrl);
                }
            }

            return replacedYaml;
        }
    }
}
