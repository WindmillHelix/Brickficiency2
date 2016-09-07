using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common
{
    public static partial class ExtensionMethods
    {
        public static byte[] ToByteArray(this Stream source)
        {
            using (var output = new MemoryStream())
            {
                source.CopyTo(output);
                var result = output.ToArray();

                return result;
            }
        }
    }
}
