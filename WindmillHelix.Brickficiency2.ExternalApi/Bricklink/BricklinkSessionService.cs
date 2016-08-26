using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Providers;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public class BricklinkSessionService : IBricklinkSessionService
    {
        private readonly IBricklinkCredentialProvider _bricklinkCredentialProvider;
        private readonly IBricklinkLoginApi _bricklinkLoginApi;

        private readonly CookieContainer _cookies = new CookieContainer();
        private readonly object _lockObject = new object();

        private DateTime? _lastLogin = null;

        public BricklinkSessionService(
            IBricklinkCredentialProvider bricklinkCredentialProvider,
            IBricklinkLoginApi bricklinkLoginApi)
        {
            _bricklinkCredentialProvider = bricklinkCredentialProvider;
            _bricklinkLoginApi = bricklinkLoginApi;
        }

        public CookieContainer GetCookieContainer()
        {
            return _cookies;
        }

        public void EnsureAuthenticated()
        {
            if (_lastLogin.HasValue && _lastLogin.Value > DateTime.Now.AddMinutes(-10))
            {
                return;
            }

            lock (_lockObject)
            {
                if (_lastLogin.HasValue && _lastLogin.Value > DateTime.Now.AddMinutes(-10))
                {
                    return;
                }

                var credentials = _bricklinkCredentialProvider.GetCredentials();
                var wasLoggedIn = _bricklinkLoginApi.Login(_cookies, credentials.UserName, credentials.Password);

                if(wasLoggedIn)
                {
                    _lastLogin = DateTime.Now;
                    return;
                }

                throw new Exception("Unable to log in to Bricklink");
            }
        }
    }
}
