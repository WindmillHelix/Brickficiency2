using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Providers;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    public class AppConfigBricklinkCredentialProvider : IBricklinkCredentialProvider
    {
        public NetworkCredential GetCredentials()
        {
            return new NetworkCredential(AppConfig.BricklinkUserName, AppConfig.BricklinkPassword);
        }
    }
}
