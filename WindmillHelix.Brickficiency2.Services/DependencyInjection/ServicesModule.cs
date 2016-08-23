using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace WindmillHelix.Brickficiency2.Services.DependencyInjection
{
    public class ServicesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .Where(f => f.Name.EndsWith("Service") || f.Name.EndsWith("Factory"))
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
