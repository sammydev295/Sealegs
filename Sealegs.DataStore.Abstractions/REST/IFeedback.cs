using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions.REST
{
    public interface IFeedback
    {
        Task<IEnumerable<FeedbackExteneded>> GetAll();
        Task<IEnumerable<Feedback>> GetAllByWine(string wineId);
        Task<Feedback> GetSingle(string lockerId);
        Task<bool> Delete(string id);
        Task<bool> Insert(Feedback feedback);
        Task<bool> Update(Feedback feedback);
    }
}
