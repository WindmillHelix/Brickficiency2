using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface ICredentialService
    {
        NetworkCredential GetCredential(ExternalSystem system);

        void SetCredential(ExternalSystem system, NetworkCredential credential);

        bool ValidateCredential(ExternalSystem system, NetworkCredential credential);
    }
}
