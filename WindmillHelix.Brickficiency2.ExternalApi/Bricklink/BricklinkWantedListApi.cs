using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkWantedListApi : IBricklinkWantedListApi
    {
        private readonly IBricklinkSessionService _bricklinkSessionService;

        public BricklinkWantedListApi(IBricklinkSessionService bricklinkSessionService)
        {
            _bricklinkSessionService = bricklinkSessionService;
        }

        public IReadOnlyCollection<WantedList> GetWantedLists()
        {
            var html = GetPageHtml("https://www.bricklink.com/v2/wanted/list.page?pageSize=10000");
            var json = ExtractJson(html);
            var lists = ParseWantedListsJson(json);

            return lists;
        }

        public IReadOnlyCollection<WantedListItem> GetWantedListItems(int wantedListId)
        {
            var url = string.Format(
                "https://www.bricklink.com/v2/wanted/search.page?wantedMoreID={0}&pageSize=10000",
                wantedListId);

            var html = GetPageHtml(url);
            var json = ExtractJson(html);
            var lists = ParseWantedListItemsJson(json);

            return lists;
        }

        private List<WantedListItem> ParseWantedListItemsJson(string json)
        {
            var reader = new JsonTextReader(new StringReader(json));
            var serializer = new JsonSerializer();

            dynamic content = serializer.Deserialize(reader);

            var results = new List<WantedListItem>();

            var itemCount = content.wantedItems.Count;
            for (int i = 0; i < content.wantedItems.Count; i++)
            {
                var wantedItem = content.wantedItems[i];
                var item = new WantedListItem();
                item.ColorId = (int)wantedItem.colorID;
                item.ItemId = (string)wantedItem.itemNo;
                item.ItemTypeCode = (string)wantedItem.itemType;
                item.Quantity = (int)wantedItem.wantedQty;

                // X = Any
                // N = New
                // U = Used
                var wantedNew = (string)wantedItem.wantedNew;
                item.Condition = wantedNew == "N" ? ItemCondition.New : ItemCondition.Used;

                results.Add(item);
            }

            return results;
        }

        private List<WantedList> ParseWantedListsJson(string json)
        {
            var reader = new JsonTextReader(new StringReader(json));
            var serializer = new JsonSerializer();

            dynamic content = serializer.Deserialize(reader);

            var results = new List<WantedList>();

            var listCount = content.wantedLists.Count;
            for(int i = 0; i < content.wantedLists.Count; i++)
            {
                var wantedList = content.wantedLists[i];
                var list = new WantedList();
                list.WantedListId = (int)wantedList.id;
                list.Name = (string)wantedList.name;

                results.Add(list);
            }

            return results;
        }

        private string ExtractJson(string html)
        {
            const string beginMarker = "var wlJson = ";
            const string endMarker = "</script>";

            var start = html.IndexOf(beginMarker) + beginMarker.Length;
            var end = html.IndexOf(endMarker, start);

            var json = html.Substring(start, end - start);
            json = json.Trim();
            if(json.EndsWith(";"))
            {
                json = json.Substring(0, json.Length - 1);
            }

            return json;
        }

        private string GetPageHtml(string url)
        {
            _bricklinkSessionService.EnsureAuthenticated();
            var cookies = _bricklinkSessionService.GetCookieContainer();

            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";

            request.UserAgent = Constants.UserAgent;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Referer = "https://www.bricklink.com/";
            request.CookieContainer = cookies;

            string html = null;
            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                html = responseReader.ReadToEnd();
            }

            return html;
        }
    }
}
