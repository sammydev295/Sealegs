using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IWines
    {
        Task<IEnumerable<Wine>> GetAllWinesById(string lockerId);
        Task<bool> DeleteWine(string id);

        Task<bool> InsertWine(Wine wine);

        Task<bool> UpdateWine(Wine wine);
        Task<Wine> GetWineById(string id);

        string GetBaseImageName(string imageUrl);
        string GetFullImageName(string imageName);
        string GetBaseSignatureName(string imageUrl);
        string GetFullSignatureName(string imageName);
    }
}
