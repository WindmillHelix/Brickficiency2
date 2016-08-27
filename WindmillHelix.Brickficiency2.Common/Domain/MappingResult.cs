using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Common.Domain
{
    public class MappingResult<T>
    {
        public bool WasSuccessful { get; set; }

        public string Message { get; set; }

        public T Mapped { get; set; }
    }
}
