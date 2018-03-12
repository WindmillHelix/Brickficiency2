using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.ExternalApi.Rebrickable;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class RebrickableTests
    {
        [TestMethod]
        public void TestDirectBricklinkAssociation()
        {
            var api = new RebrickableApi();

            var part = api.GetPartInfo("42611");

            Assert.IsNotNull(part);
            Assert.AreEqual("42611", part.PartId);
            Assert.IsNotNull(part.BricklinkItemIds);
            Assert.AreEqual(1, part.BricklinkItemIds.Count);
            Assert.AreEqual("51011", part.BricklinkItemIds[0]);
        }

        [TestMethod]
        public void TestAlternatePartIds()
        {
            var api = new RebrickableApi();

            var part = api.GetPartInfo("50746");

            Assert.IsNotNull(part);
            Assert.AreEqual("50746", part.PartId);
            Assert.IsNotNull(part.RebrickablePartIds);
            Assert.AreEqual(1, part.RebrickablePartIds.Count);
            Assert.AreEqual("54200", part.RebrickablePartIds[0]);
        }
    }
}
