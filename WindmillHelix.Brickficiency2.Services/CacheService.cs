using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    public class CacheService : ICacheService
    {
        public string GetCachedItemString(string key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllText(fileName);
        }

        public byte[] GetCachedItemBytes(string key)
        {
            var fileName = GetFileName(key);
            if (!File.Exists(fileName))
            {
                return null;
            }

            return File.ReadAllBytes(fileName);
        }

        public void WriteCachedItemString(string key, string value)
        {
            EnsureCacheFolderExists();
            var fileName = GetFileName(key);

            File.WriteAllText(fileName, value);
        }

        public void WriteCachedItemBytes(string key, byte[] value)
        {
            EnsureCacheFolderExists();
            var fileName = GetFileName(key);

            File.WriteAllBytes(fileName, value);
        }

        private void EnsureCacheFolderExists()
        {
            var folderPath = GetCacheFolderPath();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private string GetFileName(string key)
        {
            var fileName = Path.Combine(
                GetCacheFolderPath(),
                key);

            return fileName;
        }

        private string GetCacheFolderPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Brickficiency2",
                "Cache");
        }
    }
}
