using System.Collections.Generic;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IWineStore: IBaseStore<Wine>
    {
        Task<IEnumerable<Wine>> GetAllByLocker(string lockerId, bool forceRefresh);

        Task<Wine> GetSingle(int wineId);
    }
}
