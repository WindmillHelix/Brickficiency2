using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;
using WindmillHelix.Brickficiency2.Services.Calculator.NamedKeys;

namespace WindmillHelix.Brickficiency2.Services.Calculator.StepRunners
{
    internal class StoreInventoryStepRunner
    {
        public IDictionary<StoreKey, StoreInventory> GetStoreInventories(
            IDictionary<StoreKey, Store> stores,
            CalculationStep step,
            IDictionary<WantedItemKey, WantedListItem> items)
        {
            throw new NotImplementedException();
        }
    }
}
