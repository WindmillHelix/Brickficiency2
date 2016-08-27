using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.DependencyInjection;
using Autofac;
using Brickficiency.UI;

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

            var initializationForm = container.Resolve<InitializationForm>();

            Application.Run(initializationForm);
        }
    }
}
