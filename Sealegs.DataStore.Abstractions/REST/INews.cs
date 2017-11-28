using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface INews
    {
        Task<IEnumerable<News>> GetAll();
        Task<bool> DeleteNews(string id);

        Task<bool> InsertNews(News news);

        Task<bool> UpdateNews(News news);

    }
}
