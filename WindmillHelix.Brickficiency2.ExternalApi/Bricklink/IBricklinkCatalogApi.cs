using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkCatalogApi
    {
        IReadOnlyCollection<BricklinkColor> DownloadColorList();

        IReadOnlyCollection<BricklinkItemType> DownloadItemTypes();

        IReadOnlyCollection<BricklinkCategory> DownloadCategories();

        IReadOnlyCollection<BricklinkItem> DownloadItems(string itemTypeCode);
    }
}
