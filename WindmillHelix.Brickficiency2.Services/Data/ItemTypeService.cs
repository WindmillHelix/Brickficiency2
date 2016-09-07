using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class ItemTypeService : CachedDataService<ItemType>, IItemTypeService
    {
        private readonly IBricklinkCatalogApi _bricklinkCatalogService;

        public ItemTypeService(
            IAppDataService appDataService, 
            IBricklinkCatalogApi bricklinkCatalogService) 
            : base(appDataService)
        {
            _bricklinkCatalogService = bricklinkCatalogService;
        }

        protected override string AppDataKey
        {
            get
            {
                const string appDataKey = "ItemTypes_d9349d06-7228-4a9c-aa51-847e655b72b6.xml";
                return appDataKey;
            }
        }

        protected override List<ItemType> GetItemsFromSource()
        {
            var itemTypes = _bricklinkCatalogService.DownloadItemTypes();
            var converted = itemTypes.Select(x => new ItemType(x.ItemTypeCode, x.Name)).ToList();

            return converted;
        }

        public IReadOnlyCollection<ItemType> GetItemTypes()
        {
            return base.GetItems();
        }
    }
}
