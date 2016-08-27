using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    public static class AppConfig
    {
        public static string BricklinkUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["BricklinkUserName"];
            }
        }

        public static string BricklinkPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["BricklinkPassword"];
            }
        }
    }
}
