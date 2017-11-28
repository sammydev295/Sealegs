using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

using Sealegs.DataStore.Mock;

namespace Sealegs.DataStore.Mock
{
    public class FavoriteStore : BaseStore<Favorite>, IFavoriteStore
    {
        public async Task<bool> IsFavorite(string userId, string lockerId, string favoriteType)
        {
            return await Task.FromResult(Settings.IsFavorite(lockerId));
        }

        public async Task<IList<Favorite>> GetFavorites(string userId, string favoriteType)
        {
            return await Task.FromResult(new List<Favorite>());
        }

        public async override Task<bool> InsertAsync(Favorite item)
        {
            Settings.SetFavorite(item.FavoriteTypeId, true);
            return await Task.FromResult(true);
        }

        public async override Task<bool> RemoveAsync(Favorite item)
        {
            Settings.SetFavorite(item.FavoriteTypeId, false);
            return await Task.FromResult(true);
        }

        public async Task DropFavorites()
        {
            await Settings.ClearFavorites();
        }
    }
}

