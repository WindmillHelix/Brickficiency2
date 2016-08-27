using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common
{
    public class ItemType
    {
        public ItemType()
        {
        }

        public ItemType(string itemTypeCode, string name)
        {
            ItemTypeCode = itemTypeCode;
            Name = name;
        }

        public string ItemTypeCode { get; set; }

        public string Name { get; set; }
    }
}
