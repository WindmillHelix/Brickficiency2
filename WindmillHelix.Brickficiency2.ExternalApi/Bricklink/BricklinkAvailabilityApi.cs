using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Net;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkAvailabilityApi : IBricklinkAvailabilityApi
    {
        public BricklinkAvailabilityApi()
        {
        }

        public IReadOnlyCollection<BricklinkItemAvailability> GetAvailability(string itemTypeCode, string itemId,
            int colorId)
        {
            var url = string.Format(
                "https://www.bricklink.com/catalogPG.asp?{0}={1}&colorID={2}",
                HttpUtility.UrlEncode(itemTypeCode),
                HttpUtility.UrlEncode(itemId),
                colorId);

            string html;
            using (var client = new ExtendedWebClient())
            {
                client.UserAgent = Constants.UserAgent;

                html = client.DownloadString(url);
            }

            var result = ParseAvailabilityPage(html);
            return result;
        }

        private IReadOnlyCollection<BricklinkItemAvailability> ParseAvailabilityPage(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var containerTable = doc.DocumentNode.SelectSingleNode("//table[@class='fv']");

            var containerChildren = containerTable.ChildNodes?.ToList();

            if (containerChildren == null || containerChildren.Count != 4)
            {
                throw new Exception("Unexpected page structure");
            }

            var newColumn = containerChildren[3].ChildNodes.ToList()[2];
            var usedColumn = containerChildren[3].ChildNodes.ToList()[3];

            var result = new List<BricklinkItemAvailability>();
            var newItems = ParseAvailabilityColumn(newColumn, ItemCondition.New);
            var usedItems = ParseAvailabilityColumn(usedColumn, ItemCondition.Used);

            result.AddRange(newItems);
            result.AddRange(usedItems);

            return result;
        }

        private IReadOnlyCollection<BricklinkItemAvailability> ParseAvailabilityColumn(
            HtmlNode column,
            ItemCondition condition)
        {
            var rows = column.SelectNodes(".//tr[@align='RIGHT']")?.ToList();

            if (rows == null || rows.Count == 0)
            {
                return new List<BricklinkItemAvailability>();
            }

            var results = new List<BricklinkItemAvailability>();

            foreach (var row in rows)
            {
                var tds = row.ChildNodes?.ToList();
                if (tds?.Count != 3)
                {
                    continue;
                }

                var link = tds[0].SelectSingleNode("./a")?.Attributes["href"]?.Value;
                if (string.IsNullOrWhiteSpace(link) || !link.StartsWith("/store.asp"))
                {
                    continue;
                }

                var queryString = HttpUtility.ParseQueryString(link.Substring("/store.asp?".Length));
                var storeId = queryString["sID"];
                if (string.IsNullOrWhiteSpace(storeId))
                {
                    continue;
                }

                int quantity = int.Parse(tds[1].InnerText);

                var priceExtractRegex = new Regex(@".*?&nbsp;\D*([\d,]*)\.(\d+)$");
                var priceMatch = priceExtractRegex.Match(tds[2].InnerText.Replace(",", string.Empty));
                var priceString = string.Format(
                    "{0}{1}{2}",
                    priceMatch.Groups[1],
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator,
                    priceMatch.Groups[2]);

                decimal price = Convert.ToDecimal(priceString);

                var item = new BricklinkItemAvailability
                {
                    BricklinkStoreId = storeId,
                    Condition = condition,
                    Quantity = quantity,
                    Price = price
                };

                results.Add(item);
            }

            return results;
        }
    }
}
