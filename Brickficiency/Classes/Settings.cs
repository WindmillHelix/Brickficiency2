using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes
{
    public class Settings
    {
        public int nummatches;
        public int minstores;
        public int maxstores;
        public bool cont;
        public bool sortcolour;
        public bool login;
        public int splitterdistance;
        public List<string> countries = new List<string>();
        public string blacklist = "";

        public int approxtime; // added by CAC, 2015-07-08
    }
}
