using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WindmillHelix.Brickficiency2.Common;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    internal class LddFileService : ILddFileService
    {
        public IReadOnlyCollection<LddPart> ExtractPartsList(string fileName)
        {
            var xml = GetXmlFromLddFile(fileName);

            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var partNodes = doc.SelectNodes("//Part");

            List<LddPart> parts = new List<LddPart>();
            if(partNodes == null)
            {
                return parts;
            }

            foreach(XmlNode node in partNodes)
            {
                var part = new LddPart();

                part.DesignId = node.Attributes["designID"]?.InnerText;
                part.Materials = node.Attributes["materials"]?.InnerText;
                part.Decoration = node.Attributes["decoration"]?.InnerText;

                parts.Add(part);
            }

            return parts;
        }

        private string GetXmlFromLddFile(string fileName)
        {
            var zip = ZipFile.OpenRead(fileName);
            var entry = zip.Entries.Single(x => x.Name == "IMAGE100.LXFML");

            string xml;

            using (var stream = entry.Open())
            using (var reader = new StreamReader(stream))
            {
                xml = reader.ReadToEnd();
            }

            return xml;
        }
    }
}
