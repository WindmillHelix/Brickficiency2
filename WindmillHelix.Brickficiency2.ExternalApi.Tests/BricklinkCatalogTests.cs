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
        public void TestGetItemTypes()
        {
            var itemTypes = _service.DownloadItemTypes();
            Assert.IsNotNull(itemTypes);
            Assert.AreNotEqual(0, itemTypes.Count);

            var catalog = itemTypes.SingleOrDefault(x => x.ItemTypeCode == "C");
            Assert.IsNotNull(catalog);
            Assert.AreEqual("Catalog", catalog.Name);
        }

        [TestMethod]
        public void TestGetSets()
        {
            var sets = _service.DownloadItems("S");
            Assert.IsNotNull(sets);
            Assert.AreNotEqual(0, sets.Count);

            var avengersTower = sets.SingleOrDefault(x => x.ItemId == "76038-1");
            Assert.IsNotNull(avengersTower);
            Assert.AreEqual("Attack on Avengers Tower", avengersTower.Name);
        }
    }
}
