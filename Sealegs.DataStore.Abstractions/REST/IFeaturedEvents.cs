using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IFeaturedEvents
    {
        Task<IEnumerable<DataObjects.FeaturedEvent>> GetAll();
        Task<bool> DeleteFeaturedEvent(string id);

        Task<bool> InsertFeaturedEvent(DataObjects.FeaturedEvent events);

        Task<bool> UpdateFeaturedEvent(DataObjects.FeaturedEvent events);
    }
}
