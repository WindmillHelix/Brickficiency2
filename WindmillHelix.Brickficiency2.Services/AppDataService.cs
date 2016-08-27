using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    internal class AppDataService : IAppDataService
    {
        public string GetAppData(string key)
        {
            var fileName = GetFileName(key);
            if(!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllText(fileName);
        }

        public byte[] GetAppDataBytes(string key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllBytes(fileName);
        }

        public void WriteAppData(string key, string value)
        {
            EnsureAppDataFolderExists();
            var fileName = GetFileName(key);

            File.WriteAllText(fileName, value);
        }

        public void WriteAppDataBytes(string key, byte[] value)
        {
            EnsureAppDataFolderExists();
            var fileName = GetFileName(key);

            File.WriteAllBytes(fileName, value);
        }

        private void EnsureAppDataFolderExists()
        {
            var folderPath = GetAppDataFolderPath();
            if(!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private string GetFileName(string key)
        {
            var fileName = Path.Combine(
                GetAppDataFolderPath(),
                key);

            return fileName;
        }

        private string GetAppDataFolderPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Brickficiency2");
        }
    }
}
