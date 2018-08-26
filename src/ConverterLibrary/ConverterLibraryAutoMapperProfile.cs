using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using AutoMapper;
using HugoModels;
using WordpressWXR12;

namespace ConverterLibrary
{
    public class ConverterLibraryAutoMapperProfile : Profile
    {
        public const string ItemNameAttachments = "attachements";
        public const string ItemNameSiteUrl = "siteUrl";

        public ConverterLibraryAutoMapperProfile()
        {
            CreateMap<Item, PostMetadata>()
                .ForMember(metadata => metadata.Title, opt => opt.MapFrom(item => WebUtility.HtmlDecode(item.Title)))
                .ForMember(metadata => metadata.Author, opt => opt.MapFrom(item => item.Creator))
                .ForMember(metadata => metadata.Type, opt => opt.MapFrom(item => item.PostType))
                .ForMember(metadata => metadata.Excerpt, opt => opt.MapFrom(item => string.IsNullOrEmpty(item.Excerpt) ? null : item.Excerpt))
                .ForMember(metadata => metadata.Draft, opt => opt.MapFrom(item => item.Status == "draft" || item.Status == "pending" || item.Status == "private"))
                .ForMember(metadata => metadata.Private, opt => opt.MapFrom(item => item.Status == "private"))
                .ForMember(metadata => metadata.Url, opt => opt.ResolveUsing((item, metadata, x, context) => GetPermaLink(item, context.GetSiteUrl()))) 
                .ForMember(metadata => metadata.Date, opt => opt.MapFrom(item => GetDate(item)))
                .ForMember(metadata => metadata.Categories, opt => opt.MapFrom(item => item.Categories.Where(cat => cat.Domain == "category")))
                .ForMember(metadata => metadata.Tags, opt => opt.MapFrom(item => item.Categories.Where(cat => cat.Domain == "post_tag")))
                .ForMember(metadata => metadata.FeaturedImage, opt => opt.ResolveUsing((item, metadata, x, context) => GetFeaturedImage(item, context.GetAttachments(), context.GetSiteUrl())))
                ;

            CreateMap<ItemCategory, string>()
                .ProjectUsing(item => item.Text);

            CreateMap<Item, Post>()
                .ForMember(post => post.Filename, opt => opt.MapFrom(item => GetFileName(item)))
                .ForMember(post => post.Metadata, opt => opt.MapFrom(item => item));
        }


        private string GetDate(Item wordpressPost)
        {
            return ParsePostDateGmt(wordpressPost)?.ToString("yyyy-MM-ddThh:mm:sszzz", CultureInfo.InvariantCulture);
        }

        private DateTime? ParsePostDateGmt(Item wordpressPost)
        {
            DateTime? result = null;
            bool pubDateParsed = DateTime.TryParseExact(wordpressPost.PostDateGmt, new[] { "yyyy-MM-dd HH:mm:ss", "ddd, dd MMM yyyy hh:mm:ss zzz" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime pubDate);
            if (pubDateParsed)
            {
                result = pubDate;
            }

            return result;
        }

        private string GetFileName(Item wordpressPost)
        {
            string decodedPostName = string.IsNullOrEmpty(wordpressPost.PostName) ? WebUtility.UrlDecode(wordpressPost.Title.Replace(" ", "-").ToLowerInvariant()) : WebUtility.UrlDecode(wordpressPost.PostName);
            return wordpressPost.PostType == "page"
                ? $"{decodedPostName}/index.md"
                : $"{ParsePostDateGmt(wordpressPost):yyyy-MM-dd}-{decodedPostName}.md";

        }

        private string GetFeaturedImage(Item post, IDictionary<int, Item> attachments, string siteUrl)
        {
            string url = null;

            PostMeta thumbnail = post.PostMetas?.FirstOrDefault(meta => meta.Key == "_thumbnail_id");
            if (thumbnail != null)
            {
                if (int.TryParse(thumbnail.Value, out int thumbnailId))
                {
                    if (attachments.TryGetValue(thumbnailId, out Item attachment))
                    {
                        url = attachment.Guid.Text;
                    }
                }
            }

            return RemoveSiteUrl(url, siteUrl);
        }

        private string GetPermaLink(Item post, string siteUrl)
        {
            string url = null;

            if (post.PostType != "page")
            {
                url = string.IsNullOrEmpty(post.Link) ? post.Guid?.Text : post.Link;
            }

            return RemoveSiteUrl(url, siteUrl);
        }

        private string RemoveSiteUrl(string url, string siteUrl)
        {
            return url?.Replace(siteUrl, string.Empty);
        }
    }
}
