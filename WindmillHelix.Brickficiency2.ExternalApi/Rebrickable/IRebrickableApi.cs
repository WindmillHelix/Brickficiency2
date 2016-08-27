using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Rebrickable
{
    public interface IRebrickableApi
    {
        PartInfo GetPartInfo(string partNumber);
    }
}
