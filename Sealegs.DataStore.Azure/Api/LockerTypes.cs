using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class LockerTypes : ApiBase, ILockerType
    {
        public async Task<IEnumerable<DataObjects.LockerType>> GetAllLockerTypes()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.LockerType>>(GetAllLockerTypesUri);
            return result;
        }

        public async Task<LockerType> GetLockerTypeById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
