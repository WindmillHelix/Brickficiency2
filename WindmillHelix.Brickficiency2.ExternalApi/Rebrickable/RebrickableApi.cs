using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable
{
    internal class RebrickableApi : IRebrickableApi
    {
        public PartInfo GetPartInfo(string partNumber)
        {
            var url = string.Format(
                "http://rebrickable.com/api/get_part?key={0}&part_id={1}&inc_ext=1",
                HttpUtility.UrlEncode(RebrickableConstants.ApiKey),
                HttpUtility.UrlEncode(partNumber));

            string xml;
            using (var client = new WebClient())
            {
                xml = client.DownloadString(url);
            }

            if (xml.Trim() == "NOPART")
            {
                return null;
            }

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var doc = xmlDoc.DocumentElement;

            var result = new PartInfo();
            result.PartId = doc.SelectSingleNode("/root/part_id")?.InnerText;
            result.ImageUrl = doc.SelectSingleNode("/root/part_imge_url")?.InnerText;

            result.BricklinkItemIds = doc
                .SelectNodes("/root/external_part_ids/bricklink")
                ?.OfType<XmlNode>()
                .Select(x => x.InnerText)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            result.RebrickablePartIds = doc
                .SelectNodes("/root/rebrickable_part_ids/part_id")
                ?.OfType<XmlNode>()
                .Select(x => x.InnerText)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return result;
        }
    }
}
