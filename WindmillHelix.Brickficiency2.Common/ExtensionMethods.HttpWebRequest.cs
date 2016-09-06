using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common
{
    public static partial class ExtensionMethods
    {
        public static HttpWebResponse GetHttpWebResponse(this HttpWebRequest request)
        {
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException thrown)
            {
                var response = (HttpWebResponse)thrown.Response;
                return response;
            }
        }

        public static async Task<HttpWebResponse> GetHttpWebResponseAsync(this HttpWebRequest request)
        {
            try
            {
                var taskResponse = await request.GetResponseAsync();
                var response = (HttpWebResponse)taskResponse;
                return response;
            }
            catch (WebException thrown)
            {
                var response = (HttpWebResponse)thrown.Response;
                return response;
            }
        }
    }
}
