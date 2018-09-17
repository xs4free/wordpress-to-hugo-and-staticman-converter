using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ConverterLibrary.Extensions;

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

        public string Replace(string yamlContent, string siteUrl, string imageBaseUrl, out ImageReplacerResult[] replacedImages)
        {
            List<ImageReplacerResult> replacedImageResults = new List<ImageReplacerResult>();

            string replacedContent = _regexMarkdownImages.Replace(yamlContent, m =>
            {
                string title = m.Groups["title"].Value;
                string alt = m.Groups["alt"].Value;
                string url = m.Groups["url"].Value;

                string newImgTag = GetNewImgTag(title, alt, url, siteUrl, imageBaseUrl, out string originalRelativeUrl, out string newRelativeUrl);

                replacedImageResults.Add(new ImageReplacerResult
                {
                    OriginalRelativeUrl = originalRelativeUrl,
                    NewRelativeUrl = newRelativeUrl
                });

                return newImgTag;
            });

            foreach (var replacedImage in replacedImageResults)
            {
                replacedContent = ReplaceHyperlinksToImages(
                    replacedContent,
                    siteUrl.CombineUri(replacedImage.OriginalRelativeUrl),
                    replacedImage.NewRelativeUrl);
            }

            replacedImages = replacedImageResults.ToArray();
            return replacedContent;
        }

        private string GetNewImgTag(string title, string alt, string imageUrl, string siteUrl, string imageBaseUrl, out string originalRelativeUrl, out string newRelativeUrl)
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
                    newRelativeUrl = imageBaseUrl.CombineUri(imageUri.Segments.Last());

                    newImgTag = $"![{alt}]({newRelativeUrl}{titleTag})";
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
