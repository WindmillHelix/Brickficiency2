using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using System.Linq;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;
using System.Xml.Serialization;
using System.IO;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class BricklinkCatalogTests
    {
        private readonly IBricklinkCatalogService _service;

        public BricklinkCatalogTests()
        {
            _service = new BricklinkCatalogService();
        }

        [TestMethod]
        public void TestGetColors()
        {
            var colors = _service.DownloadColorList();
            Assert.IsNotNull(colors);
            Assert.AreNotEqual(0, colors.Count);

            var white = colors.SingleOrDefault(x => x.ColorId == 1);
            Assert.IsNotNull(white);
            Assert.AreEqual("White", white.Name);
            Assert.AreEqual("FFFFFF", white.Rgb);
        }

        [TestMethod]
        public void TestSerialize()
        {
            var catalog = new CatalogContainer<BricklinkColor>();
            var white = new BricklinkColor
            {
                Name = "White",
                ColorId = 1
            };

            var unknownColor = new BricklinkColor
            {
                Name = "Unknown",
                ColorId = 0
            };

            catalog.Items = new[] { white, unknownColor };
            var serializer = new XmlSerializer(typeof(CatalogContainer<BricklinkColor>));

            var writer = new StringWriter();
            serializer.Serialize(writer, catalog);
        }
    }
}
