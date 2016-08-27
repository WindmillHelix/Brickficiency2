using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkLoginApi
    {
        bool Login(CookieContainer cookies, string userName, string password);
    }
}
