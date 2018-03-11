using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculationSettings
    {
        IReadOnlyCollection<BlacklistedStore> BlacklistedStores { get; }

        bool AreAllCountriesAllowed { get; }

        IReadOnlyCollection<string> AllowedCountryCodes { get; }

        IReadOnlyCollection<string> AllowedRegionCodes { get; }
    }
}
