using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    [DebuggerDisplay("{Value}")]
    public class XmlDateTime : IXmlSerializable
    {
        public DateTime Value { get; set; }
        public bool HasValue => Value != DateTime.MinValue;
        private const string XmlDateFormat = "yyyy-MM-dd'T'HH:mm:ssZ";

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement();
                return;
            }

            string someDate = reader.ReadElementContentAsString();
            if (String.IsNullOrWhiteSpace(someDate) == false)
            {
                if (DateTime.TryParseExact(someDate, new[] { "yyyy-MM-dd HH:mm:ss", "ddd, dd MMM yyyy hh:mm:ss zzz" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime parsedDate))
                {
                    Value = parsedDate;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            if (Value == DateTime.MinValue)
            {
                return;
            }

            writer.WriteRaw(XmlConvert.ToString(Value, XmlDateFormat));
        }

        public static implicit operator DateTime(XmlDateTime custom)
        {
            return custom.Value;
        }

        public static implicit operator XmlDateTime(DateTime custom)
        {
            return new XmlDateTime { Value = custom };
        }
    }
}
