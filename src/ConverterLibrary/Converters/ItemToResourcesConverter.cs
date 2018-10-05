using System.Collections.Generic;
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

            //TODO: add other image resources (inline images and gallery images)

            return resources.ToArray();
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
