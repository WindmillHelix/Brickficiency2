using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WindmillHelix.Brickficiency2.Services.Calculator.Models
{
    internal class CalculationStep : ICalculationStep
    {
        private CalculationStepStatus _status;
        private string _name;
        private int _percentComplete;
        private int _stepOrder;

        public CalculationStep()
        {
            Status = CalculationStepStatus.Waiting;
        }

        public CalculationStepStatus Status
        {
            get
            {
                return _status;
            }

            internal set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            internal set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int PercentComplete
        {
            get
            {
                return _percentComplete;
            }

            internal set
            {
                if (_percentComplete != value)
                {
                    _percentComplete = value;
                    OnPropertyChanged();
                }
            }
        }

        public int StepOrder
        {
            get
            {
                return _stepOrder;
            }

            internal set
            {
                _stepOrder = value;
                OnPropertyChanged();
            }
        }

        internal Action Action { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
