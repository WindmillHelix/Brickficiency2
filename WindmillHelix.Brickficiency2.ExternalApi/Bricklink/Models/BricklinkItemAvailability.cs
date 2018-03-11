using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models
{
    public class BricklinkItemAvailability
    {
        public string BricklinkStoreId { get; set; }

        public int Quantity { get; set; }

        public ItemCondition Condition { get; set; }

        public decimal Price { get; set; }
    }
}
