using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface ICacheService
    {
        string GetCachedItemString(string key);

        byte[] GetCachedItemBytes(string key);

        void WriteCachedItemString(string key, string value);

        void WriteCachedItemBytes(string key, byte[] value);
    }
}
