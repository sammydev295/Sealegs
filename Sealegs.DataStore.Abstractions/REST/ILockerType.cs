using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface ILockerType
    {
        Task<IEnumerable<LockerType>> GetAllLockerTypes();
        Task<LockerType> GetLockerTypeById(string id);
    }
}
