using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface ILockerMember
    {
        Task<LockerMember> GetByMemberId(string id);

        Task<IEnumerable<LockerMember>> GetAllStaff();

        Task<IEnumerable<LockerMember>> GetAllNonStaff();

        Task<bool> DeleteLocker(string id);

        Task<bool> UpdateLocker(LockerMember locker);

        Task<bool> InsertLocker(LockerMember locker);

        string GetBaseImageName(string imageUrl);

        string GetFullImageName(string imageName);
            }
}
