using System;
using System.Threading.Tasks;

using UIKit;

using Xamarin.Forms;

using Foundation;

using Sealegs.iOS;
using Sealegs.Clients.Portable;


[assembly:Dependency(typeof(PushNotifications))]
namespace Sealegs.iOS
{
    // https://developer.xamarin.com/guides/cross-platform/application_fundamentals/notifications/ios/
    // https://developer.xamarin.com/guides/ios/application_fundamentals/notifications/remote_notifications_in_ios/
    // https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-ios-apple-push-notification-apns-get-started
    public class PushNotifications : IPushNotifications
    {
        // App ID Description: Sealges Locker App
        // Identifier: H9Y6G9YM88.com.sealegs.lockerapp
        #region IPushNotifications implementation

        public Task<bool> RegisterForNotifications()
        {
            //Settings.Current.PushNotificationsEnabled = true;
            //Settings.Current.AttemptedPush = true;

            var pushSettings = UIUserNotificationSettings.GetSettingsForTypes (
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet ());

            UIApplication.SharedApplication.RegisterUserNotificationSettings (pushSettings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications ();
            
            return Task.FromResult(true);
        }

        public bool IsRegistered
        {
            get
            {
                return UIApplication.SharedApplication.IsRegisteredForRemoteNotifications &&
                    UIApplication.SharedApplication.CurrentUserNotificationSettings.Types != UIUserNotificationType.None;
            }
        }

        public void OpenSettings()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
        }

        #endregion
    }
}

