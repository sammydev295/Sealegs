using System.Collections.Generic;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface ILockerMemberStore : IBaseStore<LockerMember>
    {
        Task<IEnumerable<LockerMember>> GetAllLockerMembers(bool force);

        Task<LockerMember> GetSingleLockerMember(int id);
    }
}
