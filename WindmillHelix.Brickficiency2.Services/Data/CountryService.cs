using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    internal class CountryService : ICountryService
    {
        private readonly List<Country> _countries = new List<Country>();

        public CountryService()
        {
            InitializeCountryList();
        }

        private void InitializeCountryList()
        {
            // todo: pull these from somewhere in the future, just trying to get them out of Main.cs right now
            Action<string, string, string> addCountry = (name, code, regionCode) =>
                        _countries.Add(new Country() {CountryCode = code, Name = name, RegionCode = regionCode});

            addCountry("Argentina", "AR", RegionCodes.Unknown);
            addCountry("Australia", "AU", RegionCodes.Unknown);
            addCountry("Austria", "AT", RegionCodes.Europe);
            addCountry("Belarus", "BY", RegionCodes.Europe);
            addCountry("Belgium", "BE", RegionCodes.Europe);
            addCountry("Bolivia", "BO", RegionCodes.Unknown);
            addCountry("Bosnia and Herzegovina", "BA", RegionCodes.Europe);
            addCountry("Brazil", "BR", RegionCodes.Unknown);
            addCountry("Bulgaria", "BG", RegionCodes.Europe);
            addCountry("Canada", "CA", RegionCodes.NorthAmerica);
            addCountry("Chile", "CL", RegionCodes.Unknown);
            addCountry("China", "CN", RegionCodes.Asia);
            addCountry("Colombia", "CO", RegionCodes.Unknown);
            addCountry("Croatia", "HR", RegionCodes.Europe);
            addCountry("Czech Republic", "CZ", RegionCodes.Europe);
            addCountry("Denmark", "DK", RegionCodes.Europe);
            addCountry("Ecuador", "EC", RegionCodes.Unknown);
            addCountry("El Salvador", "SV", RegionCodes.Unknown);
            addCountry("Estonia", "EE", RegionCodes.Europe);
            addCountry("Finland", "FI", RegionCodes.Europe);
            addCountry("France", "FR", RegionCodes.Europe);
            addCountry("Germany", "DE", RegionCodes.Europe);
            addCountry("Greece", "GR", RegionCodes.Europe);
            addCountry("Hong Kong", "HK", RegionCodes.Unknown);
            addCountry("Hungary", "HU", RegionCodes.Europe);
            addCountry("India", "IN", RegionCodes.Asia);
            addCountry("Indonesia", "ID", RegionCodes.Asia);
            addCountry("Ireland", "IE", RegionCodes.Europe);
            addCountry("Israel", "IL", RegionCodes.Unknown);
            addCountry("Italy", "IT", RegionCodes.Europe);
            addCountry("Japan", "JP", RegionCodes.Asia);
            addCountry("Latvia", "LV", RegionCodes.Europe);
            addCountry("Lithuania", "LT", RegionCodes.Europe);
            addCountry("Luxembourg", "LU", RegionCodes.Europe);
            addCountry("Macau", "MO", RegionCodes.Asia);
            addCountry("Malaysia", "MY", RegionCodes.Asia);
            addCountry("Mexico", "MX", RegionCodes.Unknown);
            addCountry("Monaco", "MC", RegionCodes.Europe);
            addCountry("Netherlands", "NL", RegionCodes.Europe);
            addCountry("New Zealand", "NZ", RegionCodes.Unknown);
            addCountry("Norway", "NO", RegionCodes.Europe);
            addCountry("Pakistan", "PK", RegionCodes.Asia);
            addCountry("Peru", "PE", RegionCodes.Unknown);
            addCountry("Philippines", "PH", RegionCodes.Asia);
            addCountry("Poland", "PL", RegionCodes.Europe);
            addCountry("Portugal", "PT", RegionCodes.Europe);
            addCountry("Romania", "RO", RegionCodes.Europe);
            addCountry("Russia", "RU", RegionCodes.Asia);
            addCountry("San Marino", "SM", RegionCodes.Europe);
            addCountry("Serbia", "RS", RegionCodes.Europe);
            addCountry("Singapore", "SG", RegionCodes.Asia);
            addCountry("Slovakia", "SK", RegionCodes.Europe);
            addCountry("Slovenia", "SI", RegionCodes.Europe);
            addCountry("South Africa", "ZA", RegionCodes.Unknown);
            addCountry("South Korea", "KR", RegionCodes.Asia);
            addCountry("Spain", "ES", RegionCodes.Europe);
            addCountry("Sweden", "SE", RegionCodes.Europe);
            addCountry("Switzerland", "CH", RegionCodes.Europe);
            addCountry("Syria", "SY", RegionCodes.Asia);
            addCountry("Taiwan", "TW", RegionCodes.Asia);
            addCountry("Thailand", "TH", RegionCodes.Asia);
            addCountry("Turkey", "TR", RegionCodes.Europe);
            addCountry("Ukraine", "UA", RegionCodes.Europe);
            addCountry("United Kingdom", "UK", RegionCodes.Europe);
            addCountry("USA", "US", RegionCodes.NorthAmerica);
            addCountry("Venezuela", "VE", RegionCodes.Unknown);
        }

        public IReadOnlyCollection<Country> GetCountries()
        {
            return _countries;
        }
    }
}
