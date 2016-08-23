using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using WindmillHelix.Brickficiency2.ExternalApi;
using WindmillHelix.Brickficiency2.Services;

namespace WindmillHelix.Brickficiency2.DependencyInjection
{
    public static class DependencyInjectionConfig
    {
        public static IContainer Setup()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(
                typeof(ExternalApiAssemblyLocator).Assembly,
                typeof(ServicesAssemblyLocator).Assembly,
                Assembly.GetEntryAssembly());

            return builder.Build();
        }
    }
}
