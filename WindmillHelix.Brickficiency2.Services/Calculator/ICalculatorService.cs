using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculatorService : INotifyPropertyChanged
    {
        void Abort();
    }
}
