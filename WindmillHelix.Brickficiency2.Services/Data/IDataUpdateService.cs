using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IDataUpdateService
    {
        DateTime? GetLastFullUpdate();

        void UpdateData();

        void PrefetchData();
    }
}
