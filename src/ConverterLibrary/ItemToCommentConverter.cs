using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using Hugo = HugoModels;
using WordpressWXR12;

namespace ConverterLibrary
{
    internal class ItemToCommentConverter : ITypeConverter<Item, IEnumerable<Hugo.Comment>>
    {
        public IEnumerable<Hugo.Comment> Convert(Item source, IEnumerable<Hugo.Comment> destination, ResolutionContext context)
        {
            if (source.Comments != null)
            {
                foreach (var comment in source.Comments.Select(cmt => context.Mapper.Map<Hugo.Comment>(cmt)))
                {
                    comment.FileName = GetCommentFileName(source, comment);
                    yield return comment;
                }
            }
        }

        private string GetCommentFileName(Item wordpressPost, Hugo.Comment hugoComment)
        {
            string decodedPostName = string.IsNullOrEmpty(wordpressPost.PostName) 
                ? WebUtility.UrlDecode(wordpressPost.Title.Replace(" ", "-").ToLowerInvariant()) 
                : WebUtility.UrlDecode(wordpressPost.PostName);

            long unixTime = hugoComment.Metadata.Date.ToUnixTime();

            return wordpressPost.PostType == "page"
                ? $"{decodedPostName}\\comment-{unixTime}.yml"
                : $"{wordpressPost.PostDateGmt.Value:yyyy-MM-dd}-{decodedPostName}\\comment-{unixTime}.yml";
        }
    }
}
