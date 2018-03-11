using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;
using WindmillHelix.Brickficiency2.Services.Calculator.NamedKeys;
using WindmillHelix.Brickficiency2.Services.Data;

namespace WindmillHelix.Brickficiency2.Services.Calculator.StepRunners
{
    internal class StoreListStepRunner
    {
        private readonly ICountryService _countryService;
        private IBricklinkStoreApi _bricklinkStoreApi;

        public StoreListStepRunner(
            ICountryService countryService,
            IBricklinkStoreApi bricklinkStoreApi)
        {
            _bricklinkStoreApi = bricklinkStoreApi;
            _countryService = countryService;
        }

        public IDictionary<StoreKey, Store> GetStores(CalculationStep step, ICalculationSettings calculationSettings)
        {
            var allCountries = _countryService.GetCountries();

            var allowedCountries =
                allCountries.Where(
                        x =>
                            calculationSettings.AreAllCountriesAllowed ||
                            calculationSettings.AllowedCountryCodes.Contains(x.CountryCode) ||
                            calculationSettings.AllowedRegionCodes.Contains(x.RegionCode))
                    .ToList();

            step.PercentComplete = 5;

            List<Store> stores = new List<Store>();

            for (int i = 0; i < allowedCountries.Count; i++)
            {
                var country = allowedCountries[i];

                var bricklinkStores = GetBricklinkStores(country);
                stores.AddRange(bricklinkStores);

                step.PercentComplete = 5 + (85 * (i + 1) / allowedCountries.Count);
            }

            var toRemove =
                stores.Where(
                        s => calculationSettings.BlacklistedStores.Any(b => b.StoreType == s.StoreType && b.Id == s.Id))
                    .ToList();

            foreach (var store in toRemove)
            {
                stores.Remove(store);
            }

            step.PercentComplete = 95;

            var result = new Dictionary<StoreKey, Store>();

            int storeKey = 1;
            foreach (var store in stores)
            {
                store.StoreKey = (StoreKey)storeKey;
                result.Add((StoreKey)storeKey, store);
                storeKey++;
            }

            return result;
        }

        private List<Store> GetBricklinkStores(Country country)
        {
            var stores = _bricklinkStoreApi.GetStoresByCountry(country.CountryCode);

            var converted = stores.Select(x => new Store
            {
                Id = x.UserName,
                Name = x.Name,
                StoreType = StoreType.BrickLink
            }).ToList();

            return converted;
        }
    }
}
