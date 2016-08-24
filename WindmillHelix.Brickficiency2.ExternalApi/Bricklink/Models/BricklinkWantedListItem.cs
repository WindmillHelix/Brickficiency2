using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkWantedListItem
    {
        public string ItemId { get; set; }

        public string ItemTypeCode { get; set; }

        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ItemCondition Condition { get; set; }
    }
}
