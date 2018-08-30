using System;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    [Serializable]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }

        [XmlElement(ElementName = "link")]
        public string Link { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "pubDate")]
        public XmlDateTime PublicationDate { get; set; }

        [XmlElement(ElementName = "language")]
        public string Language { get; set; }

        [XmlElement(ElementName = "wxr_version", Namespace = Namespaces.WP)]
        public string WxrVersion { get; set; }

        [XmlElement(ElementName = "base_site_url", Namespace = Namespaces.WP)]
        public string BaseSiteUrl { get; set; }

        [XmlElement(ElementName = "base_blog_url", Namespace = Namespaces.WP)]
        public string BaseBlogUrl { get; set; }

        [XmlElement(ElementName = "author", Namespace = Namespaces.WP)]
        public Author[] Authors { get; set; }

        [XmlElement(ElementName = "category", Namespace = Namespaces.WP)]
        public Category[] Categories { get; set; }

        [XmlElement(ElementName = "tag", Namespace = Namespaces.WP)]
        public Tag[] Tags { get; set; }

        [XmlElement(ElementName = "term", Namespace = Namespaces.WP)]
        public Term[] Terms { get; set; }

        [XmlElement(ElementName = "generator")]
        public string Generator { get; set; }

        [XmlElement(ElementName = "site", Namespace = Namespaces.WP_FEED_ADDITIONS_1)]
        public string Site { get; set; }

        [XmlElement(ElementName = "item")]
        public Item[] Items { get; set; }
    }
}
