using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class FeaturedEvents : ApiBase, IFeaturedEvents
    {
        public async Task<bool> DeleteFeaturedEvent(string id)
        {
            var api = String.Format(DeleteEventsUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

        public async Task<IEnumerable<FeaturedEvent>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<FeaturedEvent>>(GetAllEventsUri);
            return result;
        }

        public async Task<bool> InsertFeaturedEvent(FeaturedEvent events)
        {
            var form = new Dictionary<string, string>
            {
                {"Title",events.Title }, {"Description",events.Description }, {"LocationName",events.LocationName },
                {"StartTime",Convert.ToString(events.StartTime) }, {"EndTime",Convert.ToString(events.EndTime) },
                { "IsAllDay",Convert.ToString(events.IsAllDay) }
            };
            var result = await ClientBase.HttpPostRequest<bool>(InsertEventsUri, form);
            return result;
        }

        public async Task<bool> UpdateFeaturedEvent(FeaturedEvent events)
        {
            var form = new Dictionary<string, string>
            {
                {"Id",events.Id }, {"Title",events.Title }, {"Description",events.Description }, {"LocationName",events.LocationName },
                {"StartTime",Convert.ToString(events.StartTime) }, {"EndTime",Convert.ToString(events.EndTime) },
                { "IsAllDay",Convert.ToString(events.IsAllDay) }
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateEventsUri, form);
            return result;
        }
    }
}
