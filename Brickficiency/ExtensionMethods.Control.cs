using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brickficiency
{
    public static partial class ExtensionMethods
    {
        public static void InvokeAction(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}
