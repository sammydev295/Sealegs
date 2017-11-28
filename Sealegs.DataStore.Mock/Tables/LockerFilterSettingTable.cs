using Sealegs.DataStore.Abstractions.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.Utils;

namespace Sealegs.DataStore.Mock.Tables
{
    public class LockerFilterSettingTable : BaseDB, ILockerFilterSetting
    {
        public void DeleteFilterLockers()
        {
            Connection.DeleteAll<LockerFilterSettingBO>();
        }

        public LockerFilterSettingBO GetFilterLockers()
        {
            try
            {
                return Connection.Table<LockerFilterSettingBO>().FirstOrDefault();              
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void InsertFilterLocker(LockerFilterSettingBO lockerfilterSetting)
        {
            Connection.Insert(lockerfilterSetting);
        }       
    }
}
