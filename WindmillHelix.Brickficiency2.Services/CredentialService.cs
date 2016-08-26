using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.Services.Models;

namespace WindmillHelix.Brickficiency2.Services
{
    internal class CredentialService : ICredentialService
    {
        private readonly IBricklinkLoginApi _bricklinkLoginApi;
        private readonly IAppDataService _appDataService;

        public CredentialService(
            IBricklinkLoginApi bricklinkLoginApi,
            IAppDataService appDataService)
        {
            _bricklinkLoginApi = bricklinkLoginApi;
            _appDataService = appDataService;
        }

        public NetworkCredential GetCredential(ExternalSystem system)
        {
            var appData = _appDataService.GetAppData(GetAppDataKey(system));
            if(string.IsNullOrWhiteSpace(appData))
            {
                return null;
            }

            var serializer = new TypedXmlSerializer<SerializableNetworkCredential>();
            var temp = serializer.Deserialize(appData);
            var result = new NetworkCredential(temp.UserName, temp.Password, temp.Domain);

            return result;
        }

        public void SetCredential(ExternalSystem system, NetworkCredential credential)
        {
            var serializer = new TypedXmlSerializer<SerializableNetworkCredential>();
            var temp = new SerializableNetworkCredential
            {
                UserName = credential.UserName,
                Password = credential.Password,
                Domain = credential.Domain
            };

            var data = serializer.SerializeToString(temp);
            _appDataService.WriteAppData(GetAppDataKey(system), data);
        }

        public bool ValidateCredential(ExternalSystem system, NetworkCredential credential)
        {
            switch (system)
            {
                case ExternalSystem.Bricklink:
                    return _bricklinkLoginApi.Login(new CookieContainer(), credential.UserName, credential.Password);
                default:
                    throw new NotSupportedException(string.Format("The system {0} is not supported", system));
            }
        }

        private string GetAppDataKey(ExternalSystem system)
        {
            return string.Format("Credentials_{0}_86520bdf-42ec-4d79-87a7-d9b9ccde88e1", system);
        }
    }
}
