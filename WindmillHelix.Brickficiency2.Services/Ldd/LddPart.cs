using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    public class LddPart
    {
        public string DesignId { get; set; }

        public string Materials { get; set; }

        public string Decoration { get; set; }

        public override bool Equals(object obj)
        {
            var x = this;
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            var y = obj as LddPart;
            if(object.ReferenceEquals(y, null))
            {
                return false;
            }

            return x.DesignId == y.DesignId
                && x.Materials == y.Materials
                && x.Decoration == y.Decoration;
        }

        public override int GetHashCode()
        {
            return (Decoration ?? string.Empty).GetHashCode()
                ^ (DesignId ?? string.Empty).GetHashCode()
                ^ (Materials ?? string.Empty).GetHashCode();
        }
    }
}
