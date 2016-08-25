using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Brickficiency.Providers;
using Brickficiency.UI;

namespace Brickficiency.DependencyInjection
{
    public class AppModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindow>();
            builder.RegisterType<ImportWantedListForm>();
            builder.RegisterType<GetPassword>();
            builder.RegisterType<BricklinkCredentialProvider>().AsImplementedInterfaces().SingleInstance();
        }
    }
}