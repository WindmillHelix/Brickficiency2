using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Services.Data;

namespace Brickficiency.UI
{
    public partial class InitializationForm : Form
    {
        private readonly Func<MainWindow> _mainWindowFactory;
        private readonly Func<UpdateCheck> _updateConfirmationFactory;
        private readonly IDataUpdateService _dataUpdateService;

        public InitializationForm(
            IDataUpdateService dataUpdateService,
            Func<MainWindow> mainWindowFactory,
            Func<UpdateCheck> updateConfirmationFactory)
        {
            _dataUpdateService = dataUpdateService;
            _mainWindowFactory = mainWindowFactory;
            _updateConfirmationFactory = updateConfirmationFactory;

            InitializeComponent();
            StatusStrip.BackColor = Color.FromArgb(57, 109, 166);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var thread = new Thread(new ThreadStart(InitializeApplication));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void InitializeApplication()
        {
            Thread.CurrentThread.IsBackground = true;
            var watch = new Stopwatch();
            watch.Start();

            SetProgress(5);

            var lastUpdate = _dataUpdateService.GetLastFullUpdate();
            var shouldPerformDataUpdate = !lastUpdate.HasValue;
            if(lastUpdate.HasValue)
            {
                if(lastUpdate < DateTime.Now.AddDays(-20))
                {
                    // force an update after 20 days
                    shouldPerformDataUpdate = true;
                }
                else if(lastUpdate < DateTime.Now.AddDays(-10))
                {
                    // suggest an update after 10 days
                    var updateConfirmation = _updateConfirmationFactory();
                    var dialogResult = updateConfirmation.ShowDialog();
                    shouldPerformDataUpdate = dialogResult == DialogResult.OK;
                }
            }

            SetProgress(10);

            if (shouldPerformDataUpdate)
            {
                SetStatusText("Updating catalogs...");
                _dataUpdateService.UpdateData();
            }

            SetProgress(60);

            SetStatusText("Loading catalogs...");
            _dataUpdateService.PrefetchData();

            SetProgress(80);
            SetStatusText("Initializing main window...");
            var mainWindow = _mainWindowFactory();

            watch.Stop();
            var minimumWait = TimeSpan.FromSeconds(2);
            if(watch.Elapsed < minimumWait)
            {
                SetStatusText("Everything is awesome...");
                var toWait = minimumWait - watch.Elapsed;
                Thread.Sleep(toWait);
            }

            this.InvokeAction(() => this.Hide());
            mainWindow.ShowDialog();
            this.InvokeAction(() => this.Close());
        }

        private void SetStatusText(string value)
        {
            this.InvokeAction(() => StatusLabel.Text = value);
        }

        private void SetProgress(int value)
        {
            this.InvokeAction(() => InitializationProgressBar.Value = value);
        }
    }
}
