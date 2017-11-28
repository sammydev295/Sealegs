using System;
using SQLite;
using System.IO;
using Xamarin.Forms;

using Sealegs.iOS.Helpers;
using Sealegs.DataStore.Abstractions.SQLite;

[assembly: Dependency(typeof(SQLiteImplementation))]
namespace Sealegs.iOS.Helpers
{
    public class SQLiteImplementation:ISQLite
    {
        public SQLiteConnection GetConnection()
        {

            var fileName = "Sealegs.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, fileName);

          //  var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.SQLiteConnection(path);

            return connection;
        }
    }
}
