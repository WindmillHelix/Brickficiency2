using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    internal class BricklinkPriceGuideApi
    {
        private readonly IBricklinkSessionService _bricklinkSessionService;

        public BricklinkPriceGuideApi(IBricklinkSessionService bricklinkSessionService)
        {
            _bricklinkSessionService = bricklinkSessionService;
        }
    }
}
