using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace Sealegs.Backend.Models
{
    public class SealegsNotifications
    {
        public static SealegsNotifications Instance = new SealegsNotifications();

        public NotificationHubClient Hub { get; }

        public SealegsNotifications()
        {
            Hub = NotificationHubClient.CreateClientFromConnectionString(ConfigurationManager.ConnectionStrings["MS_NotificationHubConnectionString"].ConnectionString, ConfigurationManager.AppSettings["HubEndpiont"]);
        }
    }
}
