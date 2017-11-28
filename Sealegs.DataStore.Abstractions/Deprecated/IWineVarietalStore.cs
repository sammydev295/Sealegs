using System.Collections.Generic;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IWineVarietalStore : IBaseStore<WineVarietal>
    {
        Task<IEnumerable<WineVarietal>> GetAll(bool forceRefresh);
    }
}
