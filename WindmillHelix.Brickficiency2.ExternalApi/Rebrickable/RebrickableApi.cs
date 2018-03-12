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
using Newtonsoft.Json.Linq;
using WindmillHelix.Brickficiency2.ExternalApi.Rebrickable.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable
{
    internal class RebrickableApi : IRebrickableApi
    {
        public PartInfo GetPartInfo(string partNumber)
        {
            var url = string.Format(
                "https://rebrickable.com/api/v3/lego/parts/?lego_id={0}",
                HttpUtility.UrlEncode(partNumber));

            string json;
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "key " + RebrickableConstants.ApiKey);
                json = client.DownloadString(url);
            }

            var reader = new JsonTextReader(new StringReader(json));
            var serializer = new JsonSerializer();

            var searchResult = serializer.Deserialize<PartSearchResult>(reader);
            if (searchResult.Count == 0)
            {
                return null;
            }

            var part = searchResult.Results[0];
            var result = new PartInfo()
            {
                PartId = partNumber,
                ImageUrl = part.ImageUrl
            };

            result.BricklinkItemIds = new List<string>();
            result.RebrickablePartIds = new List<string>();

            if(part.PartNumber != partNumber)
            {
                result.BricklinkItemIds.Add(part.PartNumber);
                result.RebrickablePartIds.Add(part.PartNumber);
            }

            var key = part.ExternalIds?.Keys.SingleOrDefault(x => x.Equals("bricklink", StringComparison.InvariantCultureIgnoreCase));

            if(!string.IsNullOrEmpty(key))
            {
                var values = part.ExternalIds[key];
                result.BricklinkItemIds.AddRange(values.Where(x => x != part.PartNumber && x != partNumber));
            }

            return result;
        }
    }
}
