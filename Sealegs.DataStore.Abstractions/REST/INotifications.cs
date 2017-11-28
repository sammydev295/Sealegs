using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface INotifications
    {
        Task<IEnumerable<DataObjects.Notification>> GetAll();

        Task<bool> InsertNotification(DataObjects.Notification notification);

        Task<bool> UpdateNotification(DataObjects.Notification notification);

        Task<bool> DeleteNotification(string id);

        Task<bool> SendNotification(DataObjects.Notification notification);
    }
}
