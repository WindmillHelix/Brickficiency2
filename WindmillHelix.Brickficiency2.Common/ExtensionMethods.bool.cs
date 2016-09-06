using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common
{
    public static partial class ExtensionMethods
    {
        public static bool CoalesceFalse(this bool? source)
        {
            return source ?? false;
        }

        public static bool CoalesceTrue(this bool? source)
        {
            return source ?? true;
        }
    }
}
