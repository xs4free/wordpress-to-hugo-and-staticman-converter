using System;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    [Serializable, XmlRoot("rss")]
    public class RSS
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }

        [XmlElement(ElementName = "version")]
        public decimal Version { get; set; }
    }
}
