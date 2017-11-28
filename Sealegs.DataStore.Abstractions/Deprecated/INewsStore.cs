
using System.Collections.Generic;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface INewsStore : IBaseStore<News>
    {
          Task<IEnumerable<News>> GetLatestNews();
    }
}

