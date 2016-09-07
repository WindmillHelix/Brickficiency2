using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Brickficiency.Providers;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace Brickficiency
{
    public partial class MainWindow : IItemWorkbook
    {
        void IItemWorkbook.AddItem(string itemTypeCode, string itemId, int colorId, int quantity, ItemCondition condition)
        {
            var compositeId = string.Format("{0}-{1}", itemTypeCode, itemId);

            dgv_AddItem(compositeId, colorId.ToString(), quantity, condition);
        }
    }
}
