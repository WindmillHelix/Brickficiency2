using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class BricklinkItemAvailabilityTests
    {
        [TestMethod]
        public void TestGetAvailability()
        {
            var api = new BricklinkAvailabilityApi();

            var available = api.GetAvailability(ItemTypeCodes.Part, "3003", 3);
            Assert.IsNotNull(available);
            Assert.IsTrue(available.Count > 50);
        }
    }
}
