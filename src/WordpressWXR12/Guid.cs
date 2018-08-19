﻿using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class Guid
    {
        [XmlAttribute(AttributeName = "isPermaLink")]
        public bool IsPermaLink { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
