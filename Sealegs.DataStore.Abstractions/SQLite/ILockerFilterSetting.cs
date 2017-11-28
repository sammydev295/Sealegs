using Sealegs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sealegs.DataStore.Abstractions.SQLite
{
    public interface ILockerFilterSetting
    {
        void InsertFilterLocker(LockerFilterSettingBO user);
        LockerFilterSettingBO GetFilterLockers();
        void DeleteFilterLockers();
    }
}
