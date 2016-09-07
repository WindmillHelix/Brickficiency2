using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Xml;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class ColorService : CachedDataService<ItemColor>, IColorService
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
                const string appDataKey = "Colors_fa0fc98f-c808-4f93-bac6-21623cf86958.xml";
                return appDataKey;
            }
        }

        public IReadOnlyCollection<ItemColor> GetColors()
        {
            return base.GetItems();
        }

        protected override List<ItemColor> GetItemsFromSource()
        {
            var colors = _bricklinkCatalogService.DownloadColorList();
            var converted = colors.Select(x => new ItemColor()
            {
                ColorId = x.ColorId,
                Name = x.Name,
                Rgb = x.Rgb,
                ColorTypeCode = x.ColorType
            }).ToList();

            return converted;
        }
    }
}
