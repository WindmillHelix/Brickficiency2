using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Providers;
using WindmillHelix.Brickficiency2.DependencyInjection;
using WindmillHelix.Brickficiency2.Services.Calculator;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;
using WindmillHelix.Brickficiency2.Services.Ldd;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private readonly IContainer _container;

        public CalculatorTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MockSettingsProvider>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<AppConfigBricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();

            _container = DependencyInjectionConfig.Setup(builder);
        }

        private class MockSettingsProvider : ISettingsProvider
        {
            public ISettings GetSettings()
            {
                throw new NotImplementedException();
            }
        }

        private class CalculationSettings : ICalculationSettings
        {
            public CalculationSettings()
            {
                BlacklistedStores = new List<BlacklistedStore>();
                AreAllCountriesAllowed = true;
                AllowedCountryCodes = new List<string>();
                AllowedRegionCodes = new List<string>();
            }

            public IReadOnlyCollection<BlacklistedStore> BlacklistedStores { get; }

            public bool AreAllCountriesAllowed { get; }

            public IReadOnlyCollection<string> AllowedCountryCodes { get; }

            public IReadOnlyCollection<string> AllowedRegionCodes { get; }
        }

        [TestMethod]
        public void DoSimpleCalculation()
        {
            var calculator = _container.Resolve<CalculatorService>();
            Assert.IsNotNull(calculator);
            Assert.AreEqual(CalculatorStatus.NotInitialized, calculator.Status);

            calculator.Initialize(GetWantedList(), new CalculationSettings());
            Assert.AreEqual(CalculatorStatus.NotStarted, calculator.Status);

            calculator.Run();
            Assert.AreEqual(CalculatorStatus.Running, calculator.Status);

            while (calculator.Status == CalculatorStatus.Running)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private List<WantedListItem> GetWantedList()
        {
            var brick2x2ItemId = "3003";

            var result = new List<WantedListItem>();

            result.Add(new WantedListItem()
            {
                ColorId = 1,
                Condition = ItemCondition.Used,
                ItemId = brick2x2ItemId,
                ItemTypeCode = ItemTypeCodes.Part,
                Quantity = 20
            });

            result.Add(new WantedListItem()
            {
                ColorId = 2,
                Condition = ItemCondition.Used,
                ItemId = brick2x2ItemId,
                ItemTypeCode = ItemTypeCodes.Part,
                Quantity = 40
            });

            result.Add(new WantedListItem()
            {
                ColorId = 3,
                Condition = ItemCondition.Used,
                ItemId = brick2x2ItemId,
                ItemTypeCode = ItemTypeCodes.Part,
                Quantity = 60
            });

            return result;
        }
    }
}
