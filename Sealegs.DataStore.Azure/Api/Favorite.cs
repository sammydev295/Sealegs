using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoreLinq;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions.REST;

namespace Sealegs.DataStore.Azure.Api
{
    public class Favorite : ApiBase, IFavorite
    {
        public async Task<IEnumerable<DataObjects.Favorite>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.Favorite>>(GetAllFavoriteUri);
            return result;
        }

        public async Task<IEnumerable<DataObjects.Favorite>> GetAll(string Id)
        {
            var api = String.Format(GetAllFavoriteByLockerUri, Id);
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.Favorite>>(api);
            return result;
        }

        public async Task<bool> Insert(DataObjects.Favorite favorite)
        {
            var form = new Dictionary<string, string>
            {
                {"UserId", favorite.UserId },
                {"FavoriteTypeId", favorite.FavoriteTypeId },
                {"FavoriteType", favorite.FavoriteType }
            };

            var result = await ClientBase.HttpPostRequest<bool>(InsertFavoriteUri, form);
            return result;
        }

        public async Task<bool> Update(DataObjects.Favorite favorite)
        {
            var form = new Dictionary<string, string>
            {
                { "Id", favorite.Id },
                {"UserId", favorite.UserId },
                {"FavoriteTypeId", favorite.FavoriteTypeId },
                {"FavoriteType", favorite.FavoriteType }
            };

            var result = await ClientBase.HttpPostRequest<bool>(UpdateFavoriteUri, form);
            return result;
        }

        public async Task<bool> Delete(string id)
        {
            var api = String.Format(DeleteFavoriteUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

    }
}
