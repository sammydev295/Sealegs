using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using Sealegs.DataStore.Azure;

namespace Sealegs.DataStore.Azure
{
    public class NewsStore : BaseStore<News>, INewsStore
    {
        public override string Identifier => "News";

        public async Task<IEnumerable<News>> GetLatestNews()
        {
            await InitializeStore().ConfigureAwait(false);
            var items = await GetItemsAsync(true);
            return items;
        }
    }
}

