
using SQLite.Net.Interop;
using System.IO;
using TasaDeCambio.Interfaces;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(TasaDeCambio.UWP.Implementations.Config))]

namespace TasaDeCambio.UWP.Implementations
{
    public class Config : Iconfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    var fileName = "TasaDeCambio.db3";
                    var directoryDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);
             }

                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT();
                }

                return platform;
            }
        }
    }


}
