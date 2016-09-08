using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Common.Providers;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    internal class Calculator : ICalculator
    {
        private readonly ISettingsProvider _settingsProvider;

        private readonly List<WantedListItem> _items = new List<WantedListItem>();
        private readonly List<Store> _stores = new List<Store>();
        private readonly Dictionary<Guid, CalculationStep> _steps = new Dictionary<Guid, CalculationStep>();

        public Calculator(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IReadOnlyCollection<ICalculationStep> Initialize(IReadOnlyCollection<WantedListItem> items)
        {
            _items.AddRange(items);

            InitializeSteps();

            ////return _steps.Values;
            
            throw new NotImplementedException();
        }

        private void InitializeSteps()
        {
            throw new NotImplementedException();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
