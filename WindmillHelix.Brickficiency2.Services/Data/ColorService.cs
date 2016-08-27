using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class ColorService : CachedDataService<DBColour>, IColorService
    {
        private readonly IBricklinkCatalogApi _bricklinkCatalogService;

        public ColorService(
            IBricklinkCatalogApi bricklinkCatalogService,
            IAppDataService appDataService)
            : base(appDataService)
        {
            _bricklinkCatalogService = bricklinkCatalogService;
        }

        protected override string AppDataKey
        {
            get
            {
                const string appDataKey = "Colors_a676bfa9-4f23-413c-9e54-76c9816fb1bf.xml";
                return appDataKey;
            }
        }

        public IReadOnlyCollection<DBColour> GetColors()
        {
            return base.GetItems();
        }

        protected override List<DBColour> GetItemsFromSource()
        {
            var colors = _bricklinkCatalogService.DownloadColorList();
            var converted = colors.Select(x => new DBColour
            {
                id = x.ColorId.ToString(),
                name = x.Name,
                rgb = x.Rgb,
                type = x.ColorType
            }).ToList();

            return converted;
        }
    }
}
