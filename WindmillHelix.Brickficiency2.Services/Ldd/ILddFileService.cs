using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    public interface ILddFileService
    {
        IReadOnlyCollection<LddPart> ExtractPartsList(string fileName);
    }
}
