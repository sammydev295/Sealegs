using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;

using Sealegs.Backend.Models;
using Sealegs.DataObjects;
using Sealegs.Backend.Identity;
using Sealegs.Backend;

namespace Sealegs.Backend.Controllers
{
    public class CustomNotificationController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpGet]
        public IList<Notification> GetAll()
        {
            return Context.Notifications.Where(item => item.Deleted == false).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpGet]
        public Notification Get(string id)
        {
            return Context.Notifications.Where(item => item.Id.Replace("\r\n", string.Empty).ToLower() == id.ToLower()).ToList().FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpPost]
        public bool Update(Notification item)
        {
            item.UpdatedAt = DateTime.Now;
            Notification original = Context.Notifications.Find(item.Id);
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result >= 0;
            }

            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpPost]
        public bool Insert(Notification item)
        {
            item.CreatedAt = item.UpdatedAt = DateTime.Now;
            Context.Notifications.Add(item);
            int result = Context.SaveChanges();
            return result >= 0;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            Notification original = Context.Notifications.Find(id);
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(original.Deleted = true);
                int result = Context.SaveChanges();
                return result >= 0;
            }

            return false;
        }
        
        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpPost]
        public bool Send(Notification item)
        {
            HttpStatusCode ret = HttpStatusCode.InternalServerError;
            if (string.IsNullOrWhiteSpace(item.Text))//|| password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return false;

            Microsoft.Azure.NotificationHubs.NotificationOutcome outcomeAPNS = DispatchNotification(NotificationType.APNS, item.RoleId, item.Text);
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcomeGCM =  DispatchNotification(NotificationType.GCM, item.RoleId, item.Text);
            if (outcomeAPNS != null && outcomeGCM != null)
            {
                if (!((outcomeAPNS.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcomeAPNS.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown))
                    && !((outcomeGCM.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcomeGCM.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown))
                    )
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return ret == HttpStatusCode.OK;
        }

        public Microsoft.Azure.NotificationHubs.NotificationOutcome DispatchNotification(NotificationType type, string roleNames, string message)
        {
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            string[] roles = roleNames?.Split(',') ?? new String[] {}; // new String[] { "Admin" }; //
            switch (type)
            {
                case NotificationType.WNS:
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = $"<toast><visual><binding template=\"ToastText01\"><text id=\"1\">{message}</text></binding></visual></toast>";
                    outcome =  Task.Run(async () => await SealegsNotifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast)).Result;
                    break;
                case NotificationType.GCM:
                    {
                        // iOS
                        //var alert = $"{{\"aps\":{{\"alert\":\"{ message }\"}}}}";
                        var badge = 10;
                        var jsonPayload = "{\"aps\" : { \"alert\" : \"" + message + "\", \"badge\" : " + badge + ", \"sound\" : \"default\" }}";
                        outcome = Task.Run(async () => await SealegsNotifications.Instance.Hub.SendAppleNativeNotificationAsync(jsonPayload, roles)).Result;
                    }
                    break;
                case NotificationType.APNS:
                    {
                        // Android
                        //var notif = $"{{ \"data\" : {{\"message\":\"{message }\"}}}}";
                        var badge = 10;
                        var jsonPayload = "{\"data\" : { \"alert\" : \"" + message + "\", \"badge\" : " + badge + ", \"sound\" : \"default\" }}";
                        outcome = Task.Run(async () => await SealegsNotifications.Instance.Hub.SendGcmNativeNotificationAsync(jsonPayload, roles)).Result;
                    }
                    break;
            }

            return outcome;
        }
      
    }
}
