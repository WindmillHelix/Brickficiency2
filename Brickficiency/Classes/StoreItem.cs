using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes {
    public class StoreItem {
        public int qty = 0;
        public decimal price = 64000;

        public StoreItem() {
            qty = 0;
            price = 64000;
        }
        public StoreItem(int qty, decimal price) {
            this.price = price;
            this.qty = qty;
        }
    }
}
