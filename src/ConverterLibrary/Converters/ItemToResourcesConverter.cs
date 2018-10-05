using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using ConverterLibrary.Extensions;
using HugoModels;
using WordpressWXR12;

namespace ConverterLibrary.Converters
{
    public class ItemToResourcesConverter : ITypeConverter<Item, IEnumerable<Resource>>
    {
        public IEnumerable<Resource> Convert(Item source, IEnumerable<Resource> destination, ResolutionContext context)
        {
            var resources = new List<Resource>();

            var attachments = context.GetAttachments();
            var siteUrl = context.GetSiteUrl();

            resources.AddRange(GetBannerResources(source, siteUrl, attachments));
            resources.AddRange(GetImageGalleryResources(source, siteUrl, attachments));

            //other image resources (inline images) are added by the ImageReplacer-class (because it's easier to parse Markdown than HTML)

            return resources.ToArray();
        }


        /// <example>
        /// [gallery type="rectangular" size="medium" ids="926,925,916,915,924,917,918,919,920,921,923,922"]
        /// </example>>
        private static readonly string _patternGalleries = "\\[gallery type=\"(?<type>.*?)\" size=\"(?<size>.*?)\" ids=\"(?<ids>.*?)\"]";
        private readonly Regex _regexGalleries = new Regex(_patternGalleries, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private IEnumerable<Resource> GetImageGalleryResources(Item source, string siteUrl, IDictionary<int, Item> attachments)
        {
            var resources = new List<Resource>();

            _regexGalleries.Replace(source.Content ?? string.Empty, m =>
            {
                string idsText = m.Groups["ids"].Value;
                var ids = idsText.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

                foreach (var id in ids)
                {
                    if (attachments.TryGetValue(id, out Item attachment))
                    {
                        resources.Add(new Resource
                        {
                            Src = attachment.Guid.Text.RemoveBaseUrl(siteUrl),
                            Title = attachment.Title,
                            Params = new ResourceParams
                            {
                                ImageGallery = true
                            }
                        });
                    }
                }

                return string.Empty;
            });

            return resources;
        }

        private static List<Resource> GetBannerResources(Item source, string siteUrl, IDictionary<int, Item> attachments)
        {
            var resources = new List<Resource>();

            var banner = source.GetBannerImage(attachments);
            if (banner != null)
            {
                string bannerUrl = banner.Guid.Text;

                resources.Add(new Resource
                {
                    Src = bannerUrl.RemoveBaseUrl(siteUrl),
                    Title = banner.Title,
                    Params = new ResourceParams
                    {
                        Banner = true
                    }
                });
            }

            return resources;
        }
    }
}
