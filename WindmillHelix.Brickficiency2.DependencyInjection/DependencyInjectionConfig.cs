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
        public static IContainer Setup(ContainerBuilder builder = null)
        {
            if (builder == null)
            {
                builder = new ContainerBuilder();
            }

            var assemblies = new List<Assembly>();

            assemblies.Add(typeof(ExternalApiAssemblyLocator).Assembly);
            assemblies.Add(typeof(ServicesAssemblyLocator).Assembly);
            assemblies.Add(Assembly.GetEntryAssembly());

            builder.RegisterAssemblyModules(assemblies.Where(x => x != null).ToArray());

            return builder.Build();
        }
    }
}
