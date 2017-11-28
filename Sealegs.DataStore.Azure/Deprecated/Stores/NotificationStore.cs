using System;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Sealegs.DataStore.Azure
{
    public class NotificationStore  : BaseStore<Notification>, INotificationStore
    {
        public override string Identifier => "News";

        public NotificationStore()
        {
        }

        public async Task<Notification> GetLatestNotification()
        {
            await InitializeStore().ConfigureAwait(false);
            var items = await GetItemsAsync(true);
            return items.OrderByDescending(s => s.Date).ElementAt(0);
        }

        public override async Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeStore().ConfigureAwait(false);
            var server = await base.GetItemsAsync(forceRefresh).ConfigureAwait (false);
            if (server.Count () == 0) 
            {
                var items = new []
                    {
                    new Notification
                    {
                        Date = DateTime.UtcNow.AddDays(-2),
                        Text = "Don't forget to favorite your lockers so you are ready for Evolve!"
                    }
                };
                return items;
            }
            return server.OrderByDescending(s => s.Date);
        }

    }
}

