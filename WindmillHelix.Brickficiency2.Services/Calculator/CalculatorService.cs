using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Providers;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;
using WindmillHelix.Brickficiency2.Services.Calculator.NamedKeys;
using WindmillHelix.Brickficiency2.Services.Calculator.StepRunners;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    internal class CalculatorService : ICalculatorService
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly Func<StoreListStepRunner> _storeListStepRunnerFactory;
        private readonly Func<StoreInventoryStepRunner> _storeInventoryStepRunnerFactory;

        private readonly Dictionary<WantedItemKey, WantedListItem> _items = new Dictionary<WantedItemKey, WantedListItem>();
        private readonly Dictionary<Guid, CalculationStep> _steps = new Dictionary<Guid, CalculationStep>();

        private ICalculationSettings _calculationSettings;

        private CalculatorStatus _status;
        private Thread _runnerThread;

        private IDictionary<StoreKey, Store> _stores;
        private IDictionary<StoreKey, StoreInventory> _storeInventories;

        public CalculatorService(
            ISettingsProvider settingsProvider,
            Func<StoreListStepRunner> storeListStepRunnerFactory,
            Func<StoreInventoryStepRunner> storeInventoryStepRunnerFactory)
        {
            _settingsProvider = settingsProvider;
            _storeListStepRunnerFactory = storeListStepRunnerFactory;
            _storeInventoryStepRunnerFactory = storeInventoryStepRunnerFactory;

            Status = CalculatorStatus.NotInitialized;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void Initialize(IReadOnlyCollection<WantedListItem> items, ICalculationSettings calculationSettings)
        {
            _calculationSettings = calculationSettings;

            var wantedItems = items.ToList();
            for (int i = 0; i < wantedItems.Count; i++)
            {
                _items.Add((WantedItemKey)i, wantedItems[i]);
            }

            InitializeSteps();

            Status = CalculatorStatus.NotStarted;
        }

        private void InitializeSteps()
        {
            var storeListStep = new CalculationStep() { Name = "Get Store List", StepOrder = 1 };
            storeListStep.Action = () =>
            {
                var storeListStepRunner = _storeListStepRunnerFactory();
                _stores = storeListStepRunner.GetStores(storeListStep, _calculationSettings);
            };

            var storeInventoryStep = new CalculationStep() { Name = "Get Store Inventories", StepOrder = 2 };
            storeInventoryStep.Action = () =>
            {
                var storeInventoryStepRunner = _storeInventoryStepRunnerFactory();
                _storeInventories = storeInventoryStepRunner.GetStoreInventories(_stores, storeInventoryStep, _items);
            };

            _steps.Add(Guid.NewGuid(), storeListStep);
            _steps.Add(Guid.NewGuid(), storeInventoryStep);

            NotifyPropertyChanged(nameof(Steps));
        }

        public void Abort()
        {
            if (Status == CalculatorStatus.Running)
            {
                _runnerThread.Abort();
            }
        }

        public void Run()
        {
            if (Status != CalculatorStatus.NotStarted)
            {
                throw new Exception(string.Format("Calculator in incorrect status: {0}", Status));
            }

            _runnerThread = new Thread(RunWorker);
            Status = CalculatorStatus.Running;
            _runnerThread.Start();
        }

        private void RunWorker()
        {
            Thread.CurrentThread.IsBackground = true;

            var steps = _steps.Values.OrderBy(x => x.StepOrder).ToList();
            bool isFailed = false;
            foreach (var step in steps)
            {
                try
                {
                    if (isFailed)
                    {
                        step.Status = CalculationStepStatus.Skipped;
                    }
                    else
                    {
                        step.Action();
                        step.PercentComplete = 100;
                        step.Status = CalculationStepStatus.Complete;
                    }
                }
                catch (Exception thrown)
                {
                    // todo: log
                    Status = CalculatorStatus.Failed;
                    step.Status = CalculationStepStatus.Failed;
                    isFailed = true;
                }
            }
        }

        public void Cancel()
        {
            if (Status != CalculatorStatus.Running)
            {
                return;
            }

            _runnerThread.Abort();
            Status = CalculatorStatus.Cancelled;
        }

        public CalculatorStatus Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public IReadOnlyCollection<ICalculationStep> Steps
        {
            get { return _steps.Values.ToList(); }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            NotifyPropertyChanged(propertyName);
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
