using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IWineVarietal
    {
        Task<IEnumerable<WineVarietal>> GetAllWineVarietal();
        Task<bool> DeleteWineVarietal(string id);
        Task<bool> InsertWineVarietal(WineVarietal wineVarietal);
        Task<bool> UpdateWineVarietal(WineVarietal wineVarietal);
        Task<DataObjects.WineVarietal> GetWineVarietalById(string id);
    }
}
