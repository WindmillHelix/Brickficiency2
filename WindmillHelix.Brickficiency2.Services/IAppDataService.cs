using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface IAppDataService
    {
        string GetAppData(string key);

        void WriteAppData(string key, string value);
    }
}
