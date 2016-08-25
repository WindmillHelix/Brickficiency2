using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common.Domain
{
    public class WantedListItem
    {
        public string ItemId { get; set; }

        public string ItemTypeCode { get; set; }

        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ItemCondition Condition { get; set; }
    }
}
