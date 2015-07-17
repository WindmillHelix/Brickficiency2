using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes
{
    public class DBBLItem
    {
        public string id;
        public string number;
        public string type;
        public string name;
        public string weight;
        public string dimensions;
        public string catid;
        public Dictionary<string, string> pgpage = new Dictionary<string, string>();
    }
}
