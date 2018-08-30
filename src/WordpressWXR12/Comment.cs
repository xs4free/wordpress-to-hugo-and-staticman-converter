using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class Comment
    {
        [XmlElement(ElementName = "comment_id", Namespace = Namespaces.WP)]
        public int Id { get; set; }

        [XmlElement(ElementName = "comment_author", Namespace = Namespaces.WP)]
        public string Author { get; set; }

        [XmlElement(ElementName = "comment_author_email", Namespace = Namespaces.WP)]
        public string AuthorEmail { get; set; }

        [XmlElement(ElementName = "comment_author_url", Namespace = Namespaces.WP)]
        public string AuthorUrl { get; set; }

        [XmlElement(ElementName = "comment_author_IP", Namespace = Namespaces.WP)]
        public string AuthorIp { get; set; }

        [XmlElement(ElementName = "comment_date", Namespace = Namespaces.WP)]
        public XmlDateTime Date { get; set; }

        [XmlElement(ElementName = "comment_date_gmt", Namespace = Namespaces.WP)]
        public XmlDateTime DateGmt { get; set; }

        [XmlElement(ElementName = "comment_content", Namespace = Namespaces.WP)]
        public string Content { get; set; }

        [XmlElement(ElementName = "comment_approved", Namespace = Namespaces.WP)]
        public string Approved { get; set; }

        [XmlElement(ElementName = "comment_type", Namespace = Namespaces.WP)]
        public string Type { get; set; }

        [XmlElement(ElementName = "comment_parent", Namespace = Namespaces.WP)]
        public int Parent { get; set; }

        [XmlElement(ElementName = "comment_user_id", Namespace = Namespaces.WP)]
        public int UserId { get; set; }

        [XmlElement(ElementName = "commentmeta", Namespace = Namespaces.WP)]
        public CommentMeta[] Metas { get; set; }
    }
}
