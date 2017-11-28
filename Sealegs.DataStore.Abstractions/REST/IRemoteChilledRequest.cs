using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IRemoteChilledRequest
    {
        Task<IEnumerable<RemoteChillRequestExteneded>> GetAll();
        Task<IEnumerable<RemoteChillRequest>> GetAllByLockerId(string lockerId);
        Task<bool> DeleteRemoteChillRequest(string id);
        Task<bool> InsertRemoteChillRequest(RemoteChillRequest chillRequest);
        Task<bool> UpdateRemoteChillRequest(RemoteChillRequest chillRequest);
    }
}
