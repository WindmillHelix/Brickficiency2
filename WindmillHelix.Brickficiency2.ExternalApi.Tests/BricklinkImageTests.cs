using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WindmillHelix.Brickficiency2.Common;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class BricklinkImageTests
    {
        [TestMethod]
        public async Task TestNonExistentPart()
        {
            var api = new BricklinkImageApi();
            var image = await api.GetImageBytesAsync(ItemTypeCodes.Part, "8675309", 1, ImageFormat.Jpeg);

            Assert.IsNull(image);
        }

        [TestMethod]
        public async Task TestNonExistentPartLargeImage()
        {
            var api = new BricklinkImageApi();
            var image = await api.GetLargeImageBytesAsync(ItemTypeCodes.Part, "8675309", ImageFormat.Jpeg);

            Assert.IsNull(image);
        }

        [TestMethod]
        public async Task TestValidPart()
        {
            var api = new BricklinkImageApi();
            var image = await api.GetImageBytesAsync(ItemTypeCodes.Part, "15068", 11, ImageFormat.Jpeg);

            Assert.IsNotNull(image);
            Assert.AreNotEqual(0, image.Length);
        }

        [TestMethod]
        public async Task TestValidPartLargeImage()
        {
            var api = new BricklinkImageApi();
            var image = await api.GetLargeImageBytesAsync(ItemTypeCodes.Part, "15068", ImageFormat.Jpeg);

            Assert.IsNotNull(image);
            Assert.AreNotEqual(0, image.Length);
        }
    }
}
