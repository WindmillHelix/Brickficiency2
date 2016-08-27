using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes {
    public class Store {
        public Store() {
            name = "";
            items = new Dictionary<string, StoreItem>();
        }
        public Store(string storeName, Dictionary<string, StoreItem> itemList) {
            name = storeName;
            items = itemList;
        }
        private string name;
        private Dictionary<string, StoreItem> items;

        public StoreItem getItem(string id) {
            return items[id];
        }
        public decimal getPrice(string id) {
            return items[id].price;
        }

        public int getQty(string id) {
            return items[id].qty;
        }

        public string getColour(string id)
        {
            return items[id].colour;
        }

        public string getName() {
            return name;
        }
    }

    public class StoreComparer : IComparer<Store> {
        private List<Item> items;
        public StoreComparer(List<Item> items) {
            this.items = items;
        }
        public int Compare(Store x, Store y) {
            int i = 0;
            while (i < items.Count && x.getQty(items[i].extid) == y.getQty(items[i].extid)) {
                i++;
            }
            if (i == items.Count) {
                return 0;
            } else {
                // This is backwards because I want to sort descending.
                return y.getQty(items[i].extid) - x.getQty(items[i].extid);
            }
        }
    }
}
