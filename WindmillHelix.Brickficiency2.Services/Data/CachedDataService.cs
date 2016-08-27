using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Xml;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal abstract class CachedDataService<T>
    {
        private readonly IAppDataService _appDataService;

        private Lazy<bool> _isInitialized;

        private List<T> _items = new List<T>();

        public CachedDataService(IAppDataService appDataService)
        {
            _appDataService = appDataService;

            _isInitialized = new Lazy<bool>(() => EnsureInitialized());
        }

        protected abstract string AppDataKey { get; }

        protected abstract List<T> GetItemsFromSource();

        protected IReadOnlyCollection<T> GetItems()
        {
            var isInitialized = _isInitialized.Value;
            if(!isInitialized)
            {
                throw new Exception("Service failed to initialize.");
            }

            return _items;
        }

        public void Refresh()
        {
            var items = GetItemsFromSource();

            _items.Clear();
            _items.AddRange(items);

            var serializer = new TypedXmlSerializer<List<T>>();
            var serialized = serializer.SerializeToString(items);
            _appDataService.WriteAppData(AppDataKey, serialized);
        }

        private bool EnsureInitialized()
        {
            var items = GetItemsFromAppData();
            if (items == null || items.Count == 0)
            {
                Refresh();
            }
            else
            {
                _items.Clear();
                _items.AddRange(items);
            }

            return true;
        }

        private List<T> GetItemsFromAppData()
        {
            var appData = _appDataService.GetAppData(AppDataKey);
            if (string.IsNullOrWhiteSpace(appData))
            {
                return new List<T>();
            }

            var serializer = new TypedXmlSerializer<List<T>>();
            var items = serializer.Deserialize(appData);

            return items;
        }
    }
}
