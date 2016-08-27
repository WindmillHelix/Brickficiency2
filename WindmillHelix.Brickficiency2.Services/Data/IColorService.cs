using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IColorService : IRefreshable
    {
        IReadOnlyCollection<DBColour> GetColors();
    }
}
