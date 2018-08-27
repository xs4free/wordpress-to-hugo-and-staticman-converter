using System;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    [Serializable]
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        
        [XmlElement(ElementName = "pubDate")]
        public string PublicationDate { get; set; }
        
        [XmlElement(ElementName = "creator", Namespace = Namespaces.DC)]
        public string Creator { get; set; }

        [XmlElement(ElementName = "guid")]
        public Guid Guid { get; set; }
        
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "encoded", Namespace = Namespaces.CONTENT)]
        public string Content { get; set; }

        [XmlElement(ElementName = "encoded", Namespace = Namespaces.EXCERPT)]
        public string Excerpt { get; set; }
        
        [XmlElement(ElementName = "post_id", Namespace = Namespaces.WP)]
        public int PostId { get; set; }

        [XmlElement(ElementName = "post_date", Namespace = Namespaces.WP)]
        public string PostDate { get; set; }

        [XmlElement(ElementName = "post_date_gmt", Namespace = Namespaces.WP)]
        public string PostDateGmt { get; set; }

        [XmlElement(ElementName = "comment_status", Namespace = Namespaces.WP)]
        public string CommentStatus { get; set; }

        [XmlElement(ElementName = "ping_status", Namespace = Namespaces.WP)]
        public string PingStatus { get; set; }

        [XmlElement(ElementName = "post_name", Namespace = Namespaces.WP)]
        public string PostName { get; set; }

        [XmlElement(ElementName = "status", Namespace = Namespaces.WP)]
        public string Status { get; set; }

        [XmlElement(ElementName = "post_parent", Namespace = Namespaces.WP)]
        public int PostParent { get; set; }

        [XmlElement(ElementName = "menu_order", Namespace = Namespaces.WP)]
        public int MenuOrder { get; set; }

        [XmlElement(ElementName = "post_type", Namespace = Namespaces.WP)]
        public string PostType { get; set; }

        [XmlElement(ElementName = "post_password", Namespace = Namespaces.WP)]
        public string PostPassword { get; set; }

        [XmlElement(ElementName = "is_sticky", Namespace = Namespaces.WP)]
        public bool IsSticky { get; set; }
        
        [XmlElement(ElementName = "category")]
        public ItemCategory[] Categories { get; set; }

        [XmlElement(ElementName = "postmeta", Namespace = Namespaces.WP)]
        public PostMeta[] PostMetas { get; set; }

        [XmlElement(ElementName = "comment", Namespace = Namespaces.WP)]
        public Comment[] Comments { get; set; }
    }
}
