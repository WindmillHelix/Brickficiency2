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
    internal class CategoryService : CachedDataService<Category>, ICategoryService
    {
        private readonly IBricklinkCatalogApi _bricklinkCatalogService;

        public CategoryService(
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
                const string appDataKey = "Categories_0e2c17bb-79ab-4906-8cb8-deb5a9ef90d8.xml";
                return appDataKey;
            }
        }

        public IReadOnlyCollection<Category> GetCategories()
        {
            return base.GetItems();
        }

        protected override List<Category> GetItemsFromSource()
        {
            var categories = _bricklinkCatalogService.DownloadCategories();
            var converted = categories.Select(x => new Category(x.CategoryId, x.Name)).ToList();

            return converted;
        }
    }
}
