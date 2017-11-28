using System;
using Sealegs.DataObjects;
using System.Threading.Tasks;

namespace Sealegs.DataStore.Abstractions
{
    public interface INotificationStore : IBaseStore<Notification>
    {
        Task<Notification> GetLatestNotification();
    }
}

