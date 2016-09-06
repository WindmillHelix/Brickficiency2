using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IItemService : IRefreshable
    {
        IReadOnlyCollection<ItemDetails> GetItems();

        ItemDetails GetItem(string itemTypeCode, string itemId);

        IReadOnlyCollection<ItemDetails> SearchItems(string itemTypeCode, int? categoryId, string filter);
    }
}
