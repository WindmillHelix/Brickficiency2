using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface IColorService
    {
        IReadOnlyCollection<DBColour> GetColors();

        void Refresh();
    }
}
