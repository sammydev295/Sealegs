using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class WineVarietal: ApiBase, IWineVarietal
    {
        public async Task<IEnumerable<DataObjects.WineVarietal>> GetAllWineVarietal()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.WineVarietal>>(GetAllWineVarietalUri);
            return result;
        }

        public async Task<bool> DeleteWineVarietal(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertWineVarietal(DataObjects.WineVarietal wineVarietal)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateWineVarietal(DataObjects.WineVarietal wineVarietal)
        {
            throw new NotImplementedException();
        }

        public async Task<DataObjects.WineVarietal> GetWineVarietalById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
