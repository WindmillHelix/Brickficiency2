using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes {
    public class StoreItem {
        public int qty = 0;
        public decimal price = 64000;
        public string colour = "0";

        public StoreItem() {
            qty = 0;
            price = 64000;
            colour = "0";
        }
        public StoreItem(int qty, decimal price, string colour) {
            this.price = price;
            this.qty = qty;
            this.colour = colour;
        }
    }
}
