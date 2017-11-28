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
    public class Feedback : ApiBase, IFeedback
    {
        public async Task<bool> Delete(string id)
        {
            var api = String.Format(DeleteFeedbackUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

        public async Task<IEnumerable<FeedbackExteneded>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<FeedbackExteneded>>(GetAllFeedbackUri);
            result.Where(s => s.Wine.ImagePath != null).ForEach(s => s.Wine.ImagePath = String.Concat(Addresses.WinesStorageBaseAddress, s.Wine.ImagePath));

            return result;
        }

        public async Task<IEnumerable<DataObjects.Feedback>> GetAllByWine(string wineId)
        {
            var api = String.Format(GetAllFeedbackByWineUri, wineId);
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.Feedback>>(api);
            return result;
        }

        public async Task<DataObjects.Feedback> GetSingle(string id)
        {
            var api = String.Format(GetSingleFeedbackUri, id);
            var result = await ClientBase.HttpGetRequest<DataObjects.Feedback>(api);
            return result;
        }

        public async Task<bool> Insert(DataObjects.Feedback feedback)
        {
            var form = new Dictionary<string, string>
            {
                {"UserId",feedback.UserId }, {"FeedbackEntityId",feedback.FeedbackEntityId },
                { "WineId",feedback.WineId },{"Rating",Convert.ToString(feedback.Rating) },
            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertFeedbackUri, form);
            return result;
        }

        public async Task<bool> Update(DataObjects.Feedback feedback)
        {
            var form = new Dictionary<string, string>
            {
                { "Id",feedback.Id },
                { "UserId",feedback.UserId }, {"FeedbackEntityId",feedback.FeedbackEntityId },
                { "WineId",feedback.WineId },{"Rating",Convert.ToString(feedback.Rating) },
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateFeedbackUri, form);
            return result;
        }
    }
}
