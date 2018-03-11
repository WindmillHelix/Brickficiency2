using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.DependencyInjection;
using Autofac;
using WindmillHelix.Brickficiency2.Services.Data;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class ItemServiceTests
    {
        [TestMethod]
        public void TestGetItems()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfigBricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();
            var container = DependencyInjectionConfig.Setup(builder);
            var itemService = container.Resolve<IItemService>();

            Assert.IsNotNull(itemService);

            var items = itemService.GetItems();

            Assert.IsNotNull(items);
            Assert.AreNotEqual(0, items.Count);
            Assert.IsTrue(items.Count > 10000);
        }
    }
}
