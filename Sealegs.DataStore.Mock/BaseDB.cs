using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataStore.Abstractions.SQLite;
using Sealegs.Utils;
using SQLite;
using Xamarin.Forms;

namespace Sealegs.DataStore.Mock
{
    public class BaseDB
    {
        public static SQLiteConnection Connection = DependencyService.Get<ISQLite>().GetConnection();

         static BaseDB()
        {
            Connection.CreateTable<User>();
            Connection.CreateTable<LockerFilterSettingBO>();

        }
    }
}
