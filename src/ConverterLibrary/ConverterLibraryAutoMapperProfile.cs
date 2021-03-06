﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using AutoMapper;
using ConverterLibrary.Converters;
using ConverterLibrary.Extensions;
using Html2Markdown;
using HugoModels;
using WordpressWXR12;
using Comment = WordpressWXR12.Comment;

namespace ConverterLibrary
{
    public class ConverterLibraryAutoMapperProfile : Profile
    {
        public const string ItemNameAttachments = "attachements";
        public const string ItemNameSiteUrl = "siteUrl";
        public const string ItemNamePageResources = "pageResources";
        public const string HugoDateTimeFormat = "yyyy-MM-ddTHH:mm:sszzz";

        private readonly GalleryTagReplacer _galleryTagReplacer = new GalleryTagReplacer();
        private readonly CaptionTagReplacer _captionTagReplacer = new CaptionTagReplacer();

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
                .ForMember(metadata => metadata.CommentFolder, opt => opt.ResolveUsing((item, metadata, x, context) => GetCommentFolder(item)))
                .ForMember(metadata => metadata.Date, opt => opt.MapFrom(item => GetDate(item)))
                .ForMember(metadata => metadata.Categories, opt => opt.MapFrom(item => item.Categories.Where(cat => cat.Domain == "category")))
                .ForMember(metadata => metadata.Tags, opt => opt.MapFrom(item => item.Categories.Where(cat => cat.Domain == "post_tag")))
                .ForMember(metadata => metadata.Resources, opt => opt.MapFrom(item => item));

            CreateMap<Item, IEnumerable<Resource>>()
                .ConvertUsing<ItemToResourcesConverter>();

            CreateMap<ItemCategory, string>()
                .ProjectUsing(item => item.Text);

            CreateMap<Item, Post>()
                .ForMember(post => post.Filename, opt => opt.ResolveUsing((item, post, x, context) => GetFileName(item, context.UsePagedResources())))
                .ForMember(post => post.Metadata, opt => opt.MapFrom(item => item))
                .ForMember(post => post.Content, opt => opt.ResolveUsing((item, post, x, context) => GetContent(item, context.GetAttachments())));

            CreateMap<Item, IEnumerable<HugoModels.Comment>>()
                .ConvertUsing<ItemToCommentConverter>();

            CreateMap<Comment, HugoModels.Comment>()
                .ForMember(hugoComment => hugoComment.FileName, opt => opt.Ignore())
                .ForMember(hugoComment => hugoComment.Metadata, opt => opt.MapFrom(comment => comment));

            CreateMap<Comment, CommentMetadata>()
                .ForMember(metadata => metadata.Id, opt => opt.MapFrom(comment => comment.Id))
                .ForMember(metadata => metadata.Body, opt => opt.MapFrom(comment => GetBody(comment)))
                .ForMember(metadata => metadata.Date, opt => opt.MapFrom(comment => comment.Date))
                .ForMember(metadata => metadata.Email, opt => opt.MapFrom(comment => MD5Hash.Create(comment.AuthorEmail.Trim().ToLowerInvariant())))
                .ForMember(metadata => metadata.Name, opt => opt.MapFrom(comment => comment.Author))
                .ForMember(metadata => metadata.ReplyTo, opt => opt.MapFrom(comment => comment.Parent));
        }

        private string GetContent(Item item, IDictionary<int, Item> attachments)
        {
            var converter = new Converter();
            var html = item.Content;

            html = _captionTagReplacer.Replace(attachments, html);
            html = _galleryTagReplacer.Replace(attachments, html);

            var markdown = converter.Convert(html);
            return markdown; 
        }

        private string GetBody(Comment comment)
        {
            var converter = new Converter();
            var html = comment.Content;
            var markdown = converter.Convert(html);

            return markdown;
        }

        private string GetDate(Item wordpressPost)
        {
            return wordpressPost.PostDateGmt.Value.ToString(HugoDateTimeFormat, CultureInfo.InvariantCulture);
        }

        private string GetCommentFolder(Item wordPressPost)
        {
            string decodedPostName = string.IsNullOrEmpty(wordPressPost.PostName)
                ? WebUtility.UrlDecode(wordPressPost.Title.Replace(" ", "-").ToLowerInvariant())
                : WebUtility.UrlDecode(wordPressPost.PostName);

            string commentFolder = wordPressPost.PostType == "page"
                ? $"{decodedPostName}"
                : $"{wordPressPost.PostDateGmt.Value:yyyy-MM-dd}-{decodedPostName}";

            return commentFolder;
        }

        private string GetFileName(Item wordPressPost, bool usePageResources)
        {
            string commentFolder = GetCommentFolder(wordPressPost);

            string fileName = wordPressPost.PostType == "page"
                ? $"{commentFolder}\\index.md"
                : $"{commentFolder}{ (usePageResources ? "\\index" : "")}.md";

            return fileName;
        }

        private string GetPermaLink(Item post, string siteUrl)
        {
            string url = null;

            if (post.PostType != "page")
            {
                url = string.IsNullOrEmpty(post.Link) ? post.Guid?.Text : post.Link;
            }

            return url.RemoveBaseUrl(siteUrl);
        }
    }
}
