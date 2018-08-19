using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class Term
    {
        [XmlElement(ElementName = "term_id", Namespace = Namespaces.WP)]
        public string Id { get; set; }

        [XmlElement(ElementName = "term_taxonomy", Namespace = Namespaces.WP)]
        public string Taxonomy { get; set; }

        [XmlElement(ElementName = "term_slug", Namespace = Namespaces.WP)]
        public string Slug { get; set; }

        [XmlElement(ElementName = "term_parent", Namespace = Namespaces.WP)]
        public string Parent { get; set; }

        [XmlElement(ElementName = "term_name", Namespace = Namespaces.WP)]
        public string Name { get; set; }
    }
}
