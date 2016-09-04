using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public class BricklinkLoginApi : IBricklinkLoginApi
    {
        public bool Login(CookieContainer cookies, string userName, string password)
        {
            const string url = "https://www.bricklink.com/ajax/renovate/login.ajax";

            const string payloadFormat = "userid={0}&password={1}&override=false&keepme_loggedin=true";

            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";

            var payload = string.Format(
                payloadFormat,
                HttpUtility.UrlEncode(userName),
                HttpUtility.UrlEncode(password));

            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.ContentLength = payload.Length;
            request.UserAgent = Constants.UserAgent;
            request.Accept = "*/*";
            request.Referer = "https://www.bricklink.com/";
            request.CookieContainer = cookies;

            using (var requestWriter = new StreamWriter(request.GetRequestStream()))
            {
                requestWriter.Write(payload);
                requestWriter.Close();
            }

            using (var response = request.GetResponse())
            using (var responseReader = new StreamReader(response.GetResponseStream()))
            {
                var json = responseReader.ReadToEnd();

                var reader = new JsonTextReader(new StringReader(json));
                var serializer = new JsonSerializer();

                dynamic content = serializer.Deserialize(reader);
                var result =    content.returnCode.ToString() == "0"
                             || content.returnMessage.ToString() == "Already Logged-in!";

                return result;
            }
        }
    }
}
