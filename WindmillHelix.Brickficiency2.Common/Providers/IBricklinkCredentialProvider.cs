using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common.Providers
{
    public interface IBricklinkCredentialProvider
    {
        NetworkCredential GetCredentials();
    }
}
