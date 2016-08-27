using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.DependencyInjection;
using WindmillHelix.Brickficiency2.Services.Ldd;
using Autofac;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class LddMapperTests
    {
        [TestMethod]
        public void TestIndirectMapping()
        {
            var container = DependencyInjectionConfig.Setup();
            var service = container.Resolve<ILddMapperService>();

            Assert.IsNotNull(service);
            var source = new LddPart { DesignId = "50746", Materials = "1" };
            var result = service.MapItem(source);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.WasSuccessful);
            Assert.IsNotNull(result.Mapped);

            Assert.AreEqual("54200", result.Mapped.ItemId);
        }
    }
}
