using System.Collections.Generic;
using System.Text.RegularExpressions;
using WordpressWXR12;

namespace ConverterLibrary
{
    public class GalleryTagReplacer
    {
        private static readonly string _patternGalleryTags = "\\[gallery .*?]";
        private readonly Regex _regexGalleryTags = new Regex(_patternGalleryTags, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// replace gallery-tag with nothing in content
        /// </summary>
        /// <param name="attachments">{ 609, { AttachmentUrl = "https://www.progz.nl/wp-content/uploads/2017/11/img_6111.jpg", Excerpt = "Skyline van Hong Kong" }}</param>
        /// <param name="html">"[gallery type="rectangular" size="medium" ids="864,865,867,868,874,870,871,872,873"]"</param>
        /// <returns>""</returns>
        public string Replace(IDictionary<int, Item> attachments, string html)
        {
            string modified = _regexGalleryTags.Replace(html, string.Empty);

            //TODO: replace gallery-tag with img-tags

            return modified;
        }
    }
}
