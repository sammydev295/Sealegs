using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;

using Microsoft.Azure.Mobile.Server.Config;

using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    [MobileAppController]
    public class SealegsNotificationsController : ApiController
    {
        public async Task<HttpResponseMessage> Post(string pns, string password, [FromBody]string message)
        {
            Microsoft.Azure.NotificationHubs.NotificationOutcome outcome = null;
            HttpStatusCode ret = HttpStatusCode.InternalServerError;

            if (string.IsNullOrWhiteSpace(message) || password != ConfigurationManager.AppSettings["NotificationsPassword"])
                return Request.CreateResponse(ret);

            switch (pns.ToLower())
            {
                case "wns":
                    // Windows 8.1 / Windows Phone 8.1
                    var toast = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" +
                                message + "</text></binding></visual></toast>";
                    outcome = await SealegsNotifications.Instance.Hub.SendWindowsNativeNotificationAsync(toast);
                    break;
                case "apns": //  jsonPayload = "{\"aps\" : { \"alert\" : \"" + message + "\", \"badge\" : " + badge + ", \"sound\" : \"default\" }, \"acme1\" : \"bar\", \"acme2\" : 42 }";
                    // iOS
                    var alert = "{\"aps\":{\"alert\":\"" + message + "\"}}";
                    outcome = await SealegsNotifications.Instance.Hub.SendAppleNativeNotificationAsync(alert);
                    break;
                case "gcm": //  jsonPayload = "{\"data\" : { \"message\" : \"" + message + "\", \"badge\" : " + badge + ", \"sound\" : \"default\" }, \"acme1\" : \"bar\", \"acme2\" : 42 }
                    //   headers.Add("ServiceBusNotification-Tags", recipient);  +91-11-24300666
                    // Android
                    var notif = "{ \"data\" : {\"message\":\"" + message + "\"}}";
                    outcome = await SealegsNotifications.Instance.Hub.SendGcmNativeNotificationAsync(notif);
                    break;
            }

            if (outcome != null)
            {
                if (!((outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Abandoned) ||
                    (outcome.State == Microsoft.Azure.NotificationHubs.NotificationOutcomeState.Unknown)))
                {
                    ret = HttpStatusCode.OK;
                }
            }

            return Request.CreateResponse(ret);
        }
    }
}
