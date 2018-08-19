using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class Category
    {
        [XmlElement(ElementName = "term_id", Namespace = Namespaces.WP)]
        public string TermId { get; set; }

        [XmlElement(ElementName = "category_nicename", Namespace = Namespaces.WP)]
        public string NiceName { get; set; }

        [XmlElement(ElementName = "category_parent", Namespace = Namespaces.WP)]
        public string Parent { get; set; }

        [XmlElement(ElementName = "cat_name", Namespace = Namespaces.WP)]
        public string Name { get; set; }
    }
}
