using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions.REST
{
    public interface IFavorite
    {
        Task<IEnumerable<Favorite>> GetAll();
        Task<IEnumerable<Favorite>> GetAll(string Id);
        Task<bool> Delete(string id);
        Task<bool> Insert(Favorite feedback);
        Task<bool> Update(Favorite feedback);
    }
}
