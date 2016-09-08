using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculationStep : INotifyPropertyChanged
    {
        CalculationStepStatus Status { get; }

        string Name { get; }

        int PercentComplete { get; }

        int StepOrder { get; }
    }
}
