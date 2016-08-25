using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Common.Providers;

namespace Brickficiency.Providers
{
    public class BricklinkCredentialProvider : IBricklinkCredentialProvider
    {
        private readonly GetPassword _getPasswordForm;

        public BricklinkCredentialProvider(GetPassword getPasswordForm)
        {
            _getPasswordForm = getPasswordForm;
        }

        public NetworkCredential GetCredentials()
        {
            // reaching into MainWindow like this is horrible, just trying to isolate the behavior right now
            if(string.IsNullOrWhiteSpace(MainWindow.password))
            {
                var result = _getPasswordForm.ShowDialog();
                if(result != DialogResult.OK || string.IsNullOrWhiteSpace(MainWindow.password))
                {
                    throw new Exception("Password not provided");
                }
            }

            return new NetworkCredential(MainWindow.settings.username, MainWindow.password);
        }
    }
}
