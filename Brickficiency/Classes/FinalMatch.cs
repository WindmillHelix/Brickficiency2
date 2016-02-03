using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Brickficiency.Classes {

    public class FinalMatch {
        private SerializableDictionary<string, MatchDetails> itemDictionary = new SerializableDictionary<string, MatchDetails>();

        public void AddStore(string name) {
            itemDictionary.Add(name, new MatchDetails());
        }
        public decimal totalprice {
            get {
                if (itemDictionary.Count == 0) {
                    return 0;
                }
                decimal tp = 0;
                foreach (MatchDetails md in itemDictionary.Values) {
                    tp += md.totalStorePrice;
                }
                return tp;
            }
        }

        public int num {
            get { return itemDictionary.Count; }
        }

        public List<string> GetStoreNames() {
            return itemDictionary.Keys.ToList();
        }

        public MatchDetails GetDetails(string store) {
            return itemDictionary[store];
        }
    }

    public class MatchDetails {
        public MatchDetails() {
            this.totalStorePrice = 0;
            this.totalNumberItemsFromStore = 0;
            this.totalNumberLotsFromStore = 0;
            this.list = "";
            this.xml = "";
        }

        public void AddItem(string id, int qty, decimal price, string colour) {
            try {
	            itemDictionary.Add(id, new StoreItem(qty, price, colour));
	            totalStorePrice += price * qty;
            }
            catch (ArgumentException) {
                System.Diagnostics.Debug.Assert(false, "An element with id \"" + id + "\" already exists.");
            }
        }

        public StoreItem GetItem(string id) {
            if (itemDictionary.ContainsKey(id)) {
                return itemDictionary[id];
            }
            return null;
        }
        private Dictionary<string, StoreItem> itemDictionary = new Dictionary<string, StoreItem>();
        public decimal totalStorePrice = 0;
        public int totalNumberItemsFromStore = 0;
        public int totalNumberLotsFromStore = 0;
        public string list = "";
        public string xml = "";
    }
}
