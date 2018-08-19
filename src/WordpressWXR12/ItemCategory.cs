using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class ItemCategory
    {
        [XmlAttribute(AttributeName = "domain")]
        public string Domain { get; set; }

        [XmlAttribute(AttributeName = "nicename")]
        public string NiceName { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
