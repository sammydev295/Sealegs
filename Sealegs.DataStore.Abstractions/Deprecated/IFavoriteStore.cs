using System;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using System.Collections.Generic;

namespace Sealegs.DataStore.Abstractions
{
    public interface IFavoriteStore : IBaseStore<Favorite>
    {
        Task<bool> IsFavorite(string userId, string lockerId, string favoriteType);
        Task<IList<Favorite>> GetFavorites(string userId, string favoriteType);
        Task DropFavorites();
    }
}

