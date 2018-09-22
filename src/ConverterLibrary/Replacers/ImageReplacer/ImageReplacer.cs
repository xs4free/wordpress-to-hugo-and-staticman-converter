﻿using System;
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

        public IEnumerable<ImageReplacerResult> Replace(Post hugoPost, string siteUrl, string imageBaseUrl)
        {
            List<ImageReplacerResult> replacedImageResults = new List<ImageReplacerResult>();

            ReplaceImageLinksInContent(siteUrl, imageBaseUrl, hugoPost, replacedImageResults);
            ReplaceLinksToImagesInContent(siteUrl, replacedImageResults, hugoPost);
            ReplaceBannerLink(hugoPost, imageBaseUrl, replacedImageResults);

            return replacedImageResults;
        }

        private static void ReplaceBannerLink(Post hugoPost, string imageBaseUrl, List<ImageReplacerResult> replacedImageResults)
        {
            if (!string.IsNullOrEmpty(hugoPost?.Metadata?.Banner))
            {
                var originalRelativeUrl = hugoPost.Metadata.Banner;
                var bannerSegments = hugoPost.Metadata.Banner.Split('/', StringSplitOptions.RemoveEmptyEntries);

                var newRelativeUrl = imageBaseUrl.CombineUri(bannerSegments.Last());
                hugoPost.Metadata.Banner = newRelativeUrl;

                replacedImageResults.Add(new ImageReplacerResult
                {
                    OriginalRelativeUrl = originalRelativeUrl,
                    NewRelativeUrl = newRelativeUrl
                });
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
            List<ImageReplacerResult> replacedImageResults)
        {
            string replacedContent = _regexMarkdownImages.Replace(hugoPost.Content ?? String.Empty, m =>
            {
                string title = m.Groups["title"].Value;
                string alt = m.Groups["alt"].Value;
                string url = m.Groups["url"].Value;

                string newImgTag = GetNewImgTag(title, alt, url, siteUrl, imageBaseUrl, out string originalRelativeUrl,
                    out string newRelativeUrl);

                replacedImageResults.Add(new ImageReplacerResult
                {
                    OriginalRelativeUrl = originalRelativeUrl,
                    NewRelativeUrl = newRelativeUrl
                });

                return newImgTag;
            });

            hugoPost.Content = replacedContent;
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