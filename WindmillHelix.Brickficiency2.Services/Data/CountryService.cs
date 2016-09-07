using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Action<string, string> addCountry =
                (name, code) => _countries.Add(new Country() { CountryCode = code, Name = name });

            addCountry("Argentina", "AR");
            addCountry("Australia", "AU");
            addCountry("Austria", "AT");
            addCountry("Belarus", "BY");
            addCountry("Belgium", "BE");
            addCountry("Bolivia", "BO");
            addCountry("Bosnia and Herzegovina", "BA");
            addCountry("Brazil", "BR");
            addCountry("Bulgaria", "BG");
            addCountry("Canada", "CA");
            addCountry("Chile", "CL");
            addCountry("China", "CN");
            addCountry("Colombia", "CO");
            addCountry("Croatia", "HR");
            addCountry("Czech Republic", "CZ");
            addCountry("Denmark", "DK");
            addCountry("Ecuador", "EC");
            addCountry("El Salvador", "SV");
            addCountry("Estonia", "EE");
            addCountry("Finland", "FI");
            addCountry("France", "FR");
            addCountry("Germany", "DE");
            addCountry("Greece", "GR");
            addCountry("Hong Kong", "HK");
            addCountry("Hungary", "HU");
            addCountry("India", "IN");
            addCountry("Indonesia", "ID");
            addCountry("Ireland", "IE");
            addCountry("Israel", "IL");
            addCountry("Italy", "IT");
            addCountry("Japan", "JP");
            addCountry("Latvia", "LV");
            addCountry("Lithuania", "LT");
            addCountry("Luxembourg", "LU");
            addCountry("Macau", "MO");
            addCountry("Malaysia", "MY");
            addCountry("Mexico", "MX");
            addCountry("Monaco", "MC");
            addCountry("Netherlands", "NL");
            addCountry("New Zealand", "NZ");
            addCountry("Norway", "NO");
            addCountry("Pakistan", "PK");
            addCountry("Peru", "PE");
            addCountry("Philippines", "PH");
            addCountry("Poland", "PL");
            addCountry("Portugal", "PT");
            addCountry("Romania", "RO");
            addCountry("Russia", "RU");
            addCountry("San Marino", "SM");
            addCountry("Serbia", "RS");
            addCountry("Singapore", "SG");
            addCountry("Slovakia", "SK");
            addCountry("Slovenia", "SI");
            addCountry("South Africa", "ZA");
            addCountry("South Korea", "KR");
            addCountry("Spain", "ES");
            addCountry("Sweden", "SE");
            addCountry("Switzerland", "CH");
            addCountry("Syria", "SY");
            addCountry("Taiwan", "TW");
            addCountry("Thailand", "TH");
            addCountry("Turkey", "TR");
            addCountry("Ukraine", "UA");
            addCountry("United Kingdom", "UK");
            addCountry("USA", "US");
            addCountry("Venezuela", "VE");
        }

        public IReadOnlyCollection<Country> GetCountries()
        {
            return _countries;
        }
    }
}
