using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.DependencyInjection;
using Autofac;

namespace Brickficiency
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = DependencyInjectionConfig.Setup();
            var window = container.Resolve<MainWindow>();

            Application.Run(window);
        }
    }
}
