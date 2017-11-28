using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class Notifications : ApiBase, INotifications
    {
        public async Task<IEnumerable<Notification>> GetAll()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<Notification>>(GetAllNotificationsUri);
            return result;
        }

        public async Task<bool> InsertNotification(Notification notification)
        {
            var form = new Dictionary<string, string>
            {
                 {"Text", notification.Text }, {"Date", Convert.ToString(notification.Date) }, {"RoleId", notification.RoleId}
            };

            var result = await ClientBase.HttpPostRequest<bool>(InsertNotificationsUri, form);
            return result;
        }

        public async Task<bool> UpdateNotification(Notification notification)
        {
            var form = new Dictionary<string, string>
            {
                {"Id", notification.Id }, {"Text", notification.Text }, {"Date", Convert.ToString(notification.Date) }, {"RoleId", notification.RoleId}
            };
            var result = await ClientBase.HttpPostRequest<bool>(UpdateNotificationsUri, form);
            return result;
        }

        public async Task<bool> DeleteNotification(string id)
        {
            var api = String.Format(DeleteNotificationsUri, id);
            var result = await ClientBase.HttpGetRequest<bool>(api);
            return result;
        }

        public async Task<bool> SendNotification(DataObjects.Notification notification)
        {
            var form = new Dictionary<string, string>
            {
               {"Id", notification.Id }, {"Text", notification.Text }, {"Date", Convert.ToString(notification.Date) }, {"RoleId", notification.RoleId}
            };

            var result = await ClientBase.HttpPostRequest<bool>(SendNotificationsUri, form);
            return result;
        }
    }
}
