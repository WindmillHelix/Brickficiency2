using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class ItemService : CachedDataService<ItemDetails>, IItemService
    {
        private readonly IBricklinkCatalogApi _bricklinkCatalogService;
        private readonly IItemTypeService _itemTypeService;

        public ItemService(
            IAppDataService appDataService,
            IBricklinkCatalogApi bricklinkCatalogService,
            IItemTypeService itemTypeService) 
            : base(appDataService)
        {
            _bricklinkCatalogService = bricklinkCatalogService;
            _itemTypeService = itemTypeService;
        }

        protected override string AppDataKey
        {
            get
            {
                const string appDataKey = "Items_ed2bc0c0-b9c4-440c-8dd0-3bc1bac95496.xml";
                return appDataKey;
            }
        }

        protected override List<ItemDetails> GetItemsFromSource()
        {
            var itemTypes = _itemTypeService.GetItemTypes();
            var bricklinkItems = new List<BricklinkItem>();

            foreach (var itemType in itemTypes.Where(x => x.ItemTypeCode != "U"))
            {
                var toAdd = _bricklinkCatalogService.DownloadItems(itemType.ItemTypeCode);
                bricklinkItems.AddRange(toAdd);
            }

            var converted = bricklinkItems.Select(x => ConvertItem(x)).ToList();

            return converted;
        }

        private static ItemDetails ConvertItem(BricklinkItem source)
        {
            string dimensions = null;
            if(!string.IsNullOrWhiteSpace(source.DimensionX))
            {
                dimensions = string.Format(
                    "{0}x{1}x{2}", 
                    source.DimensionX, 
                    source.DimensionY, 
                    source.DimensionZ);
            }

            var target = new ItemDetails
            {
                CategoryId = source.CategoryId,
                Dimensions = dimensions,
                ItemId = source.ItemId,
                ItemTypeCode = source.ItemTypeCode,
                Name = source.Name,
                Weight = source.Weight,
                Year = source.Year
            };

            return target;
        }

        IReadOnlyCollection<ItemDetails> IItemService.GetItems()
        {
            return base.GetItems();
        }

        void IRefreshable.Refresh()
        {
            base.Refresh();
        }
    }
}
