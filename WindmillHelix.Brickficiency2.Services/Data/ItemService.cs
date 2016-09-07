using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class ItemService : CachedDataService<ItemDetails>, IItemService
    {
        private readonly IBricklinkCatalogApi _bricklinkCatalogService;
        private readonly IItemTypeService _itemTypeService;
        private readonly List<Tuple<string, ItemDetails>> _searchList = new List<Tuple<string, ItemDetails>>();

        private IDictionary<string, List<int>> _itemTypeToCategoryIds = new Dictionary<string, List<int>>();

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

        public ItemDetails GetItem(string itemTypeCode, string itemId)
        {
            // todo: populate and use dictionaries for this
            return GetItems().SingleOrDefault(x => x.ItemTypeCode == itemTypeCode && x.ItemId == itemId);
        }

        public IReadOnlyCollection<ItemDetails> SearchItems(string itemTypeCode, int? categoryId, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter) && !categoryId.HasValue)
            {
                return new List<ItemDetails>();
            }

            filter = filter.ToLowerInvariant();

            var watch = Stopwatch.StartNew();

            var filtered = _searchList
                .Where(
                    x =>
                        x.Item2.ItemTypeCode == itemTypeCode &&
                        (!categoryId.HasValue || categoryId.Value == x.Item2.CategoryId) && x.Item1.Contains(filter))
                .Select(x => x.Item2)
                .Distinct()
                .ToList();

            watch.Stop();

            return filtered;
        }

        public IReadOnlyCollection<int> GetCategoryIdsForItemType(string itemTypeCode)
        {
            if (!_itemTypeToCategoryIds.ContainsKey(itemTypeCode))
            {
                return new List<int>();
            }

            return _itemTypeToCategoryIds[itemTypeCode];
        }

        protected override void AfterItemsModified(IReadOnlyCollection<ItemDetails> items)
        {
            base.AfterItemsModified(items);

            RebuildSearchList(items);
            RebuildPartTypeToCategoryMap(items);
        }

        private void RebuildSearchList(IReadOnlyCollection<ItemDetails> items)
        {
            var names = items.Select(x => Tuple.Create(x.Name.ToLowerInvariant(), x));

            // allow searching for for "1x8" insteaad of always having to do "1 x 8"
            var shorterNames = items.Select(x => Tuple.Create(x.Name.ToLowerInvariant().Replace(" x ", "x"), x));

            var itemIds = items.Select(x => Tuple.Create(x.ItemId.ToLowerInvariant(), x));

            var searchList = new List<Tuple<string, ItemDetails>>();
            searchList.AddRange(names);
            searchList.AddRange(shorterNames);
            searchList.AddRange(itemIds);

            searchList = searchList.Distinct().ToList();

            _searchList.Clear();
            _searchList.AddRange(searchList);
        }

        private void RebuildPartTypeToCategoryMap(IReadOnlyCollection<ItemDetails> items)
        {
            var relations = from p in items
                          group p.CategoryId by p.ItemTypeCode into g
                          select new { ItemTypeCode = g.Key, CategoryIds = g.Distinct().ToList() };

            var newDictionary = relations.ToDictionary(x => x.ItemTypeCode, x => x.CategoryIds);

            _itemTypeToCategoryIds = newDictionary;
        }
    }
}
