using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common.Utils
{
    public static class ItemTypeUtil
    {
        public static bool DoesComeInColors(string itemTypeCode)
        {
            return itemTypeCode == ItemTypeCodes.Part || itemTypeCode == ItemTypeCodes.Gear;
        }
    }
}
