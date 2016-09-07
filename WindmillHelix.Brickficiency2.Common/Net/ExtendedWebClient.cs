using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common.Net
{
    public class ExtendedWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            var webRequest = request as HttpWebRequest;
            if (webRequest == null)
            {
                return request;
            }

            webRequest.UserAgent = UserAgent;

            return webRequest;
        }

        public string UserAgent { get; set; }
    }
}
