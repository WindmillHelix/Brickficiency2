using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using WindmillHelix.Brickficiency2.Common.Net;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkStoreApi : IBricklinkStoreApi
    {
        public BricklinkStoreApi()
        {
        }

        public IReadOnlyCollection<BricklinkStore> GetStoresByCountry(string countryCode)
        {
            string html;

            using (var client = new ExtendedWebClient())
            {
                client.UserAgent = Constants.UserAgent;
                var url = string.Format(
                    "https://www.bricklink.com/browseStores.asp?countryID={0}",
                    HttpUtility.UrlEncode(countryCode));

                html = client.DownloadString(url);
            }

            var stores = ParseStoreList(html);
            return stores;
        }

        private IReadOnlyCollection<BricklinkStore> ParseStoreList(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var links = doc.DocumentNode.SelectNodes("//a").ToList();

            var stores = new List<BricklinkStore>();

            foreach (var link in links)
            {
                var href = link.Attributes["href"];
                if (string.IsNullOrWhiteSpace(href?.Value))
                {
                    continue;
                }

                if (!href.Value.StartsWith("store.asp?"))
                {
                    continue;
                }

                var storeName = link.InnerText;
                var queryString = HttpUtility.ParseQueryString(href.Value.Substring("store.asp?".Length));
                var storeId = queryString["p"];

                var store = new BricklinkStore()
                {
                    UserName = storeId,
                    Name = storeName
                };

                stores.Add(store);
            }

            return stores;
        }
    }
}
