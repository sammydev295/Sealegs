using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using System.ServiceModel;
using MoreLinq;
using Sealegs.Clients.Portable;
using Xamarin.Forms;

namespace Sealegs.DataStore.Azure.Api
{
    public class RemoteChilledRequest :ApiBase, IRemoteChilledRequest
    {
        public Task<bool> DeleteRemoteChillRequest(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RemoteChillRequestExteneded>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<RemoteChillRequestExteneded>>(GetAllRemoteChillRequestUri);
           result.Where(s => s.Wine.ImagePath != null).ForEach(s => s.Wine.ImagePath = String.Concat(Addresses.WinesStorageBaseAddress, s.Wine.ImagePath));

            return result;
        }

        public async Task<IEnumerable<RemoteChillRequest>> GetAllByLockerId(string lockerId)
        {
            var api = String.Format(GetAllRemoteChillRequestByLockerIdUri, lockerId);
            var result = await ClientBase.HttpGetRequest<IEnumerable<RemoteChillRequest>>(api);
            return result;
        }

        public async Task<bool> InsertRemoteChillRequest(RemoteChillRequest chillRequest)
        {
            var form = new Dictionary<string, string>
            {
                {"RemoteChillRequestID",chillRequest.RemoteChillRequestID }, {"LockerMemberID",chillRequest.LockerMemberID },
                { "MemberBottleID",chillRequest.MemberBottleID },{"RequestDate",Convert.ToString(chillRequest.RequestDate) },
                { "Description",chillRequest.Description }, {"IsCompleted",Convert.ToString(chillRequest.IsCompleted) },
                { "CompletedByID",Convert.ToString(chillRequest.CompletedByID) } 
            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertRemoteChillRequestUri, form);
            return result;
        }

        public async Task<bool> UpdateRemoteChillRequest(RemoteChillRequest chillRequest)
        {
            var form = new Dictionary<string, string>
            {
                { "Id",chillRequest.Id},
                { "RemoteChillRequestID",chillRequest.RemoteChillRequestID }, {"LockerMemberID",chillRequest.LockerMemberID },
                { "MemberBottleID",chillRequest.MemberBottleID },{"RequestDate",Convert.ToString(chillRequest.RequestDate) },
                { "Description",chillRequest.Description }, {"IsCompleted",Convert.ToString(chillRequest.IsCompleted) },
                { "CompletedByID",Convert.ToString(chillRequest.CompletedByID) }
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateRemoteChillRequestUri, form);
            return result;
        }
    }
}
