using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Xml;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class DataUpdateService : IDataUpdateService
    {
        private List<Action> _prefetchActions = new List<Action>();
        private List<IRefreshable> _refreshables = new List<IRefreshable>();

        private IAppDataService _appDataService;

        private const string _appDataKey = "DataUpdate_7416f8aa-6255-4750-a987-69a7bfdc82f7";
        private const string _lastUpdatePropertyName = "LastUpdate";

        public DataUpdateService(
            IColorService colorService,
            IItemService itemService,
            IItemTypeService itemTypeService,
            ICategoryService categoryService,
            IAppDataService appDataService)
        {
            _appDataService = appDataService;

            _refreshables.Add(colorService);
            _refreshables.Add(itemService);
            _refreshables.Add(itemTypeService);
            _refreshables.Add(categoryService);

            _prefetchActions.Add(() => colorService.GetColors());
            _prefetchActions.Add(() => itemService.GetItems());
            _prefetchActions.Add(() => itemTypeService.GetItemTypes());
            _prefetchActions.Add(() => categoryService.GetCategories());
        }

        public DateTime? GetLastFullUpdate()
        {
            var properties = ReadProperties();

            if(!properties.ContainsKey(_lastUpdatePropertyName))
            {
                return null;
            }

            return DateTime.Parse(properties[_lastUpdatePropertyName]);
        }

        private IDictionary<string, string> ReadProperties()
        {
            var appData = _appDataService.GetAppData(_appDataKey);
            if (string.IsNullOrWhiteSpace(appData))
            {
                return new Dictionary<string, string>();
            }

            var serializer = new TypedXmlSerializer<List<SerializableKeyValuePair<string, string>>>();
            var list = serializer.Deserialize(appData);
            return list.ToDictionary(x => x.Key, x => x.Value);
        }

        private void WriteProperties(IDictionary<string, string> properties)
        {
            var list = properties.Select(x => new SerializableKeyValuePair<string, string>(x.Key, x.Value)).ToList();
            var serializer = new TypedXmlSerializer<List<SerializableKeyValuePair<string, string>>>();
            var data = serializer.SerializeToString(list);
            _appDataService.WriteAppData(_appDataKey, data);
        }

        public void PrefetchData()
        {
            foreach(var action in _prefetchActions)
            {
                action();
            }
        }

        public void UpdateData()
        {
            foreach(var item in _refreshables)
            {
                item.Refresh();
            }

            var properties = ReadProperties();
            properties[_lastUpdatePropertyName] = DateTime.Now.ToString();
            WriteProperties(properties);
        }
    }
}
