using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindmillHelix.Brickficiency2.DependencyInjection;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    [TestClass]
    public class BricklinkWantedListTests
    {
        private IBricklinkWantedListApi _api;

        [TestInitialize]
        public void Setup()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AppConfigBricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();

            var container = DependencyInjectionConfig.Setup(builder);
            _api = container.Resolve<IBricklinkWantedListApi>();
        }

        [TestMethod]
        public void TestGetWantedLists()
        {
            var wantedLists = _api.GetWantedLists();

            Assert.IsNotNull(wantedLists);
            Assert.AreNotEqual(0, wantedLists.Count);
        }

        [TestMethod]
        public void TestGetWantedListItems()
        {
            var wantedLists = _api.GetWantedLists();

            Assert.IsNotNull(wantedLists);
            Assert.AreNotEqual(0, wantedLists.Count);

            foreach(var wantedList in wantedLists)
            {
                var items = _api.GetWantedListItems(wantedList.WantedListId);
            }
        }
    }
}
