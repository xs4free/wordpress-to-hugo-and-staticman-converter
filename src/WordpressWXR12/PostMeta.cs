using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class PostMeta
    {
        [XmlElement(ElementName = "meta_key", Namespace = Namespaces.WP)]
        public string Key { get; set; }
        
        [XmlElement(ElementName = "meta_value", Namespace = Namespaces.WP)]
        public string Value { get; set; }
    }
}
