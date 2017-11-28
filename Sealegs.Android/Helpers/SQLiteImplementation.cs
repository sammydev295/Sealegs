using SQLite;
using System.IO;
using Xamarin.Forms;

using Sealegs.Droid.Helpers;
using Sealegs.DataStore.Abstractions.SQLite;


[assembly: Dependency(typeof(SQLiteImplementation))]
namespace Sealegs.Droid.Helpers
{
    public class SQLiteImplementation : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var fileName = "Sealegs.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, fileName);

            //  var platform = new SQLite.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.SQLiteConnection(path);

            return connection;
        }
    }
}