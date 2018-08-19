using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class Tag
    {
        [XmlElement(ElementName = "term_id", Namespace = Namespaces.WP)]
        public string TermId { get; set; }

        [XmlElement(ElementName = "tag_slug", Namespace = Namespaces.WP)]
        public string Slug { get; set; }
        
        [XmlElement(ElementName = "tag_name", Namespace = Namespaces.WP)]
        public string Name { get; set; }
    }
}
