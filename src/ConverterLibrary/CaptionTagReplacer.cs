using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WordpressWXR12;

namespace ConverterLibrary
{
    public class CaptionTagReplacer
    {
        private static readonly string _patternImgCaptions = "\\[caption id=\"attachment_(?<attachment_id>.*?)\".*?\\]<img.*?\\[/caption]";
        private readonly Regex _regexImgCaptions = new Regex(_patternImgCaptions, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// replace caption-tag with img-tag in content
        /// </summary>
        /// <param name="attachments">{ 609, { AttachmentUrl = "https://www.progz.nl/wp-content/uploads/2017/11/img_6111.jpg", Excerpt = "Skyline van Hong Kong" }}</param>
        /// <param name="html">"[caption id="attachment_609" align="aligncenter" width="697"]<img class="wp-image-609 size-large" src="https://www.progz.nl/wp-content/uploads/2017/11/img_6111-1024x683.jpg" alt="" width="697" height="465" /> Skyline van Hong Kong[/caption]"</param>
        /// <returns>"<img url="https://www.progz.nl/wp-content/uploads/2017/11/img_6111.jpg" alt="Skyline van Hong Kong" />"</returns>
        public string Replace(IDictionary<int, Item> attachments, string html)
        {
            return _regexImgCaptions.Replace(html, m => GetImgTag(m.Groups["attachment_id"].Value, attachments));
        }

        private string GetImgTag(string attachmentId, IDictionary<int, Item> attachments)
        {
            string imgTag = string.Empty;

            int id = Convert.ToInt32(attachmentId);
            if (attachments.TryGetValue(id, out Item attachment))
            {
                imgTag = $"<img src=\"{attachment.AttachmentUrl}\" alt=\"{attachment.Excerpt}\" />";
            }

            return imgTag;
        }
    }
}