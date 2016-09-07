using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindmillHelix.Brickficiency2.Common.Domain;

namespace Brickficiency.Providers
{
    public interface IItemWorkbook
    {
        void AddItem(
            string itemTypeCode,
            string itemId,
            int colorId = 0,
            int quantity = 0,
            ItemCondition condition = ItemCondition.Used);
    }
}
