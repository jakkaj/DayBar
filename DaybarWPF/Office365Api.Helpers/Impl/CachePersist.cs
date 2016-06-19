using System;
using System.IO;
using DayBar.Contract.Repo;

namespace Office365Api.Helpers.Impl
{
    public class CachePersist : ICachePersist
    {
        public void Write(byte[] cache)
        {
            var f = new FileInfo(_cacheFile());

            if (!f.Directory.Exists)
            {
                f.Directory.Create();
            }   

            File.WriteAllBytes(f.FullName,cache);
        }

        public byte[] Read()
        {
            var f = new FileInfo(_cacheFile());

            if (!f.Exists)
            {
                return null;
            }

            return File.ReadAllBytes(f.FullName);
        }

        string _cacheFile()
        {
            var ad = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var file = $"{ad}\\daybar\\cache.dat";

            return file;
        }
    }
}
