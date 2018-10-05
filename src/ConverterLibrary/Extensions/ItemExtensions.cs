using System.Collections.Generic;
using System.Linq;
using WordpressWXR12;

namespace ConverterLibrary.Extensions
{
    public static class ItemExtensions
    {
        public static Item GetBannerImage(this Item post, IDictionary<int, Item> attachments)
        {
            Item image = null;

            PostMeta thumbnail = post.PostMetas?.FirstOrDefault(meta => meta.Key == "_thumbnail_id");
            if (thumbnail != null)
            {
                if (int.TryParse(thumbnail.Value, out int thumbnailId))
                {
                    attachments.TryGetValue(thumbnailId, out image);
                }
            }

            return image;
        }
    }
}
