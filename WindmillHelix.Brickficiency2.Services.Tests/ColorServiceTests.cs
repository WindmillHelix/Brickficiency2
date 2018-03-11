using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;
using WindmillHelix.Brickficiency2.Services;
using System.Linq;
using WindmillHelix.Brickficiency2.Services.Data;
using Autofac;
using WindmillHelix.Brickficiency2.DependencyInjection;

namespace WindmillHelix.Brickficiency2.Services.Tests
{
    [TestClass]
    public class ColorServiceTests
    {
        private readonly IColorService _service;

        public ColorServiceTests()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfigBricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();

            var container = DependencyInjectionConfig.Setup(builder);
            _service = container.Resolve<IColorService>();
        }

        [TestMethod]
        public void TestGetColors()
        {
            var colors = _service.GetColors();
            Assert.IsNotNull(colors);
            Assert.AreNotEqual(0, colors.Count);

            var white = colors.SingleOrDefault(x => x.ColorId == 1);
            Assert.IsNotNull(white);
            Assert.AreEqual("White", white.Name);
            Assert.AreEqual("FFFFFF", white.Rgb);
        }
    }
}
