using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkCatalogService : IBricklinkCatalogService
    {
        public IReadOnlyCollection<BricklinkColor> DownloadColorList()
        {
            const string postString = "itemType=S&viewType=3&itemTypeInv=S&itemNo=&downloadType=X";
            var result = DownloadCatalog<BricklinkColor>(postString);

            return result;
        }

        public IReadOnlyCollection<BricklinkItemType> DownloadItemTypes()
        {
            const string postString = "itemType=S&viewType=1&itemTypeInv=S&itemNo=&downloadType=X";
            var result = DownloadCatalog<BricklinkItemType>(postString);

            return result;
        }

        public IReadOnlyCollection<BricklinkCategory> DownloadCategories()
        {
            const string postString = "itemType=S&viewType=2&itemTypeInv=S&itemNo=&downloadType=X";
            var result = DownloadCatalog<BricklinkCategory>(postString);

            return result;
        }

        public IReadOnlyCollection<BricklinkItem> DownloadItems(string itemTypeCode)
        {
            const string postStringFormat = "viewType=0&itemType={0}&selYear=Y&selWeight=Y&selDim=Y&itemTypeInv=S&itemNo=&downloadType=X";
            var postString = string.Format(postStringFormat, HttpUtility.UrlEncode(itemTypeCode));
            var result = DownloadCatalog<BricklinkItem>(postString);

            return result;
        }

        private IReadOnlyCollection<T> DownloadCatalog<T>(string postString)
        {
            const string contentType = "application/x-www-form-urlencoded";

            var request = (HttpWebRequest)HttpWebRequest.Create("https://www.bricklink.com/catalogDownload.asp?a=a");
            request.Method = "POST";

            request.ContentType = contentType;

            request.ContentLength = postString.Length;
            request.UserAgent = Constants.UserAgent;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Referer = "https://www.bricklink.com/catalogDownload.asp";

            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(postString);
                requestWriter.Close();
            }

            string xml = null;
            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                xml = responseReader.ReadToEnd();
            }

            var serializer = new TypedXmlSerializer<CatalogContainer<T>>();
            var deserialized = serializer.Deserialize(xml);

            return deserialized.Items;
        }
    }
}
