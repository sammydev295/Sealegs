using System.Collections.Generic;
using System.Threading.Tasks;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure
{
    public class WineVarietalStore : BaseStore<WineVarietal>, IWineVarietalStore
    {
        public override string Identifier => "WineVarietals";

        public async Task<IEnumerable<WineVarietal>> GetAll(bool forceRefresh = false )
        {
            await InitializeStore().ConfigureAwait(false);

            if (forceRefresh)
                await PullLatestAsync().ConfigureAwait(false);
        
            var wineVarietals = await Table.ToListAsync().ConfigureAwait(false);
            return wineVarietals;
        }
    }
}
