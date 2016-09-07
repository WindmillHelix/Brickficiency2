using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services;
using WindmillHelix.Brickficiency2.Services.Data;

namespace Brickficiency.UI
{
    public partial class InitializationForm : Form
    {
        private readonly Func<MainWindow> _mainWindowFactory;
        private readonly Func<UpdateCheck> _updateConfirmationFactory;
        private readonly IDataUpdateService _dataUpdateService;
        private readonly ICredentialService _credentialService;

        public InitializationForm(
            IDataUpdateService dataUpdateService,
            Func<MainWindow> mainWindowFactory,
            Func<UpdateCheck> updateConfirmationFactory,
            ICredentialService credentialService)
        {
            _dataUpdateService = dataUpdateService;
            _mainWindowFactory = mainWindowFactory;
            _updateConfirmationFactory = updateConfirmationFactory;
            _credentialService = credentialService;

            InitializeComponent();
            StatusStrip.BackColor = Color.FromArgb(57, 109, 166);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            var thread = new Thread(InitializeApplication);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private void InitializeApplication()
        {
            Thread.CurrentThread.IsBackground = true;
            var watch = new Stopwatch();
            watch.Start();

            SetProgress(5);

            DoDataUpdate();
            SetProgress(50);

            ValidateSavedCredentials();
            SetProgress(65);

            PrefetchCatalogs();
            SetProgress(80);

            SetStatusText("Initializing main window...");
            var mainWindow = _mainWindowFactory();

            watch.Stop();
            EnsureMinimumWait(watch);
            SetProgress(100);

            this.InvokeAction(() => this.Hide());
            mainWindow.ShowDialog();
            this.InvokeAction(() => this.Close());
        }

        private void DoDataUpdate()
        {
            var lastUpdate = _dataUpdateService.GetLastFullUpdate();
            var shouldPerformDataUpdate = !lastUpdate.HasValue;
            if (lastUpdate.HasValue)
            {
                if (lastUpdate < DateTime.Now.AddDays(-20))
                {
                    // force an update after 20 days
                    shouldPerformDataUpdate = true;
                }
                else if (lastUpdate < DateTime.Now.AddDays(-10))
                {
                    // suggest an update after 10 days
                    var updateConfirmation = _updateConfirmationFactory();
                    var dialogResult = updateConfirmation.ShowDialog();
                    shouldPerformDataUpdate = dialogResult == DialogResult.OK;
                }
            }

            if (shouldPerformDataUpdate)
            {
                SetStatusText("Updating catalogs...");
                _dataUpdateService.UpdateData();
            }
        }

        private void PrefetchCatalogs()
        {
            SetStatusText("Loading catalogs...");
            _dataUpdateService.PrefetchData();
        }

        private void EnsureMinimumWait(Stopwatch watch)
        {
            var minimumWait = TimeSpan.FromSeconds(2);
            if (watch.Elapsed < minimumWait)
            {
                SetStatusText("Everything is awesome...");
                var toWait = minimumWait - watch.Elapsed;
                Thread.Sleep(toWait);
            }
        }

        private void ValidateSavedCredentials()
        {
            SetStatusText("Finding saved credentials...");
            var systems = Enum.GetValues(typeof(ExternalSystem)).OfType<ExternalSystem>().ToList();
            var toValidate = new List<Tuple<ExternalSystem, NetworkCredential>>();

            foreach (var system in systems)
            {
                var credential = _credentialService.GetCredential(system);
                if (credential != null && !string.IsNullOrWhiteSpace(credential.Password))
                {
                    toValidate.Add(new Tuple<ExternalSystem, NetworkCredential>(system, credential));
                }
            }

            if(toValidate.Count == 0)
            {
                return;
            }

            SetStatusText("Validating saved credentials...");
            foreach (var item in toValidate)
            {
                var system = item.Item1;
                var credential = item.Item2;

                bool isValid = _credentialService.ValidateCredential(system, credential);
                if(!isValid)
                {
                    // remove any saved passwords that are no longer valid so we don't lock out accounts
                    credential.Password = null;
                    _credentialService.SetCredential(system, credential);
                }
            }
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
