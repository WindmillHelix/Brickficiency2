using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Providers;
using WindmillHelix.Brickficiency2.Services;

namespace Brickficiency.Providers
{
    public class BricklinkCredentialProvider : IBricklinkCredentialProvider
    {
        private readonly Func<GetPassword> _getPasswordFormFactory;
        private readonly ICredentialService _credentialService;

        private string _userName;
        private string _password;

        public BricklinkCredentialProvider(
            ICredentialService credentialService, 
            Func<GetPassword> getPasswordFormFactory)
        {
            _getPasswordFormFactory = getPasswordFormFactory;
            _credentialService = credentialService;
        }

        public NetworkCredential GetCredentials()
        {
            if (string.IsNullOrWhiteSpace(_password) && string.IsNullOrWhiteSpace(_userName))
            {
                var credential = _credentialService.GetCredential(ExternalSystem.Bricklink);
                if (credential != null)
                {
                    _userName = credential.UserName;
                    _password = credential.Password;
                }
            }

            if (!string.IsNullOrWhiteSpace(_password) && !string.IsNullOrWhiteSpace(_userName))
            {
                return new NetworkCredential(_userName, _password);
            }

            var form = _getPasswordFormFactory();
            var dialogResult = form.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                _userName = form.UserName;
                _password = form.Password;

                return new NetworkCredential(_userName, _password);
            }

            return null;
        }
    }
}
