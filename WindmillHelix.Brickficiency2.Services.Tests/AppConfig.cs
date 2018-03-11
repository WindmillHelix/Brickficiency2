using System.Configuration;

namespace WindmillHelix.Brickficiency2.Services.Tests
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
