using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common
{
    public static class ParseUtil
    {
        public static int? TryParseInt(string value)
        {
            int temp;
            bool didParse = int.TryParse(value, out temp);

            return didParse ? temp : (int?)null;
        }
    }
}
