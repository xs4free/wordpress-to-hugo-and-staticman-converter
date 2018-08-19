using System;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    [Serializable]
    public class Author
    {
        [XmlElement(ElementName = "author_id", Namespace = Namespaces.WP)]
        public string Id { get; set; }

        [XmlElement(ElementName = "author_login", Namespace = Namespaces.WP)]
        public string Login { get; set; }

        [XmlElement(ElementName = "author_email", Namespace = Namespaces.WP)]
        public string Email { get; set; }

        [XmlElement(ElementName = "author_display_name", Namespace = Namespaces.WP)]
        public string DisplayName { get; set; }

        [XmlElement(ElementName = "author_first_name", Namespace = Namespaces.WP)]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "author_last_name", Namespace = Namespaces.WP)]
        public string LastName { get; set; }
    }
}
