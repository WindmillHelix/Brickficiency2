using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WindmillHelix.Brickficiency2.Common.Xml
{
    public class TypedXmlSerializer<T>
    {
        private readonly XmlSerializer _serializer;

        public TypedXmlSerializer()
        {
            _serializer = new XmlSerializer(typeof(T));
        }

        public void SerializeToStream(Stream destination, T value)
        {
            _serializer.Serialize(destination, value);
        }

        public string SerializeToString(T value)
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }

        public T Deserialize(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var temp = _serializer.Deserialize(reader);
                return (T)temp;
            }
        }
    }
}
