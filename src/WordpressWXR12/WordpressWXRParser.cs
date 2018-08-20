using System.IO;
using System.Xml.Serialization;

namespace WordpressWXR12
{
    public class WordpressWXRParser
    {
        public RSS LoadFromFile(string fileName)
        {
            XmlSerializer ser = new XmlSerializer(typeof(RSS));
            using (FileStream myFileStream = new FileStream(fileName, FileMode.Open))
            {
                return (RSS)ser.Deserialize(myFileStream);
            }
        }
    }
}
