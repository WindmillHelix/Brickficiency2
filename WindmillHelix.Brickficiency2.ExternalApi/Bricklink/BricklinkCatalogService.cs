using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkCatalogService : IBricklinkCatalogService
    {
        public IReadOnlyCollection<BricklinkColor> DownloadColorList()
        {
            const string contentType = "application/x-www-form-urlencoded";

            var request = (HttpWebRequest)HttpWebRequest.Create("https://www.bricklink.com/catalogDownload.asp?a=a");
            request.Method = "POST";

            var postString = "itemType=S&viewType=3&itemTypeInv=S&itemNo=&downloadType=X";

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

            var serializer = new TypedXmlSerializer<CatalogContainer<BricklinkColor>>();
            var deserialized = serializer.Deserialize(xml);

            return deserialized.Items;
        }
    }
}
