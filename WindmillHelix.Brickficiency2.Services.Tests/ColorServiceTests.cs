using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.Services;
using System.Linq;
using WindmillHelix.Brickficiency2.Services.Data;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class ColorServiceTests
    {
        private readonly IColorService _service;

        public ColorServiceTests()
        {
            _service = new ColorService(
                new BricklinkCatalogApi(),
                new AppDataService());
        }

        [TestMethod]
        public void TestGetColors()
        {
            var colors = _service.GetColors();
            Assert.IsNotNull(colors);
            Assert.AreNotEqual(0, colors.Count);

            var white = colors.SingleOrDefault(x => x.id == "1");
            Assert.IsNotNull(white);
            Assert.AreEqual("White", white.name);
            Assert.AreEqual("FFFFFF", white.rgb);
        }
    }
}
