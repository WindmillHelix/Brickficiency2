using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    public interface ILddMapperService
    {
        MappingResult<MappedPart> MapItem(LddPart source);
    }
}
