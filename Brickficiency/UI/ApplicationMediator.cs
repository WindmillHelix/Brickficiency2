using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brickficiency.UI
{
    public class ApplicationMediator
    {
        private readonly Func<ImportLddForm> _importLddFormFactory;

        public ApplicationMediator(Func<ImportLddForm> importLddFormFactory)
        {
            _importLddFormFactory = importLddFormFactory;
        }

        public void ImportLddFile()
        {
            var form = _importLddFormFactory();
            form.ShowDialog();
        }
    }
}
