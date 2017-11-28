using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using Sealegs.DataStore.Azure;


namespace Sealegs.DataStore.Azure
{
    public class FavoriteStore : BaseStore<Favorite>, IFavoriteStore
    {
        public async  Task<bool> IsFavorite(string userId, string lockerId, string favoriteType)
        {
            await InitializeStore().ConfigureAwait (false);
            var items = await Table.Where(s => s.UserId == userId && s.FavoriteTypeId == lockerId && s.FavoriteType == favoriteType).ToListAsync().ConfigureAwait (false);
            return items.Count > 0;
        }
        public async Task<IList<Favorite>> GetFavorites(string userId, string favoriteType)
        {
            await InitializeStore().ConfigureAwait(false);
            var items = await Table.Where(s => s.UserId == userId && s.FavoriteType == favoriteType).ToListAsync().ConfigureAwait(false);
            return items;
        }

        public Task DropFavorites()
        {
            return Task.FromResult(true);
        }

        public override string Identifier => "Favorite";
    }
}

