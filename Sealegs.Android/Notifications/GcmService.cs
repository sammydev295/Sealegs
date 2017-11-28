using System;
using System.Linq;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Support.V4.App;

using Gcm;
using WindowsAzure.Messaging;

using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Droid
{
    // https://components.xamarin.com/gettingstarted/gcmclient
    // https://developer.xamarin.com/guides/cross-platform/application_fundamentals/notifications/android/
    // https://developer.xamarin.com/guides/android/application_fundamentals/notifications/remote-notifications-with-gcm/
    // gcm package name: com.sealegs.lockerapp
    // gcm server api key: AIzaSyD3032vz499Lscl6P5Ul5Ib9oq6GZitNl4
    // gcm sender id: 381575223842
    [Service(Name = "com.sealegs.lockerapp.GcmService")] // Must use the service tag
    public class GcmService : GcmServiceBase
    {
        // The Azure notification Hub
        static NotificationHub hub;

        #region Initialize (static)

        public static void Initialize(Context context)
        {
            try
            {
                // Call this from our main activity - not needed per latest articles
                //var cs = ConnectionString.CreateUsingSharedAccessKeyWithListenAccess (
                //    new Java.Net.URI (ApiKeys.AzureListenConnection), 
                //    ApiKeys.AzureListenConnection);

                hub = new NotificationHub (ApiKeys.AzureHubName, ApiKeys.AzureListenConnection, context);
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "GcmService.Initialize();";
            }
        }

        #endregion 

        #region Register (static)

        public static void Register(Context context)
        {
            try
            {
                // Makes this easier to call from our Activity
                GcmClient.Register(context, GcmBroadcastReceiver.SENDERIDS);
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "GcmService.Register();";
                Console.WriteLine("Unable to register GCMClient" + ex);
            }
        }

        #endregion 

        #region CTOR

        public GcmService() : base(GcmBroadcastReceiver.SENDERIDS)
        {
        }

        #endregion 

        #region OnRegistered / OnUnRegistered

        protected override void OnRegistered(Context context, string registrationId)
        {
            // Receive registration Id for sending GCM Push Notifications to
            try
            {
                List<string> tags = (BaseViewModel.User?.Role != null) ? Sealegs.DataObjects.SealegsUserRole.RoleHierarchy.ToList().SkipWhile(rs => rs.Id.ToLower() != BaseViewModel.User?.Role.Id.ToLower()).Select(r => r.RoleName).ToList() : new List<string>();
                if (hub != null)
                    hub.Register(registrationId, tags.ToArray());

                Settings.Current.PushRegistered = true;
            }
            catch (Exception ex)
            {
                ex.Data["method"] = "GcmService.OnRegistered();";
                Console.WriteLine("Unable to register Hub" + ex);
            }
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            try
            {
                if (hub != null)
                    hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to unregister" + ex);
            }

        }

        #endregion 

        #region OnMessage

        protected override void OnMessage(Context context, Intent intent)
        {
            Console.WriteLine("Received Notification");

            try
            {
                // Push Notification arrived - print out the keys/values
                if (intent != null || intent.Extras != null)
                {
                    var keyset = intent.Extras.KeySet();
                    foreach (var key in keyset)
                    {
                        var message = intent.Extras.GetString(key);
                        Console.WriteLine("Key: {0}, Value: {1}", key, message);
                        if (key == "message" || key == "alert")
                            ShowNotification(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing message: " + ex);
            }

        }

        void ShowNotification(string message)
        {
            try
            {
                Console.WriteLine("SendNotification");
                var notificationManager = NotificationManagerCompat.From(this);

                Console.WriteLine("Created Manager");
                var notificationIntent = new Intent(this, typeof(MainActivity));
                notificationIntent.AddFlags(ActivityFlags.ClearTop | ActivityFlags.NewTask);
                var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

                Console.WriteLine("Created Pending Intent");
                /*var wearableExtender =
                    new NotificationCompat.WearableExtender()
                        .SetBackground(BitmapFactory.DecodeResource(Resources, Resource.Drawable.ic_background_evolve));*/

                var style = new NotificationCompat.BigTextStyle();
                style.BigText(message);

                var builder = new NotificationCompat.Builder(this)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle("Sealegs Notification")
                    .SetAutoCancel(true)
                    .SetStyle(style)
                    .SetSmallIcon(Resource.Drawable.ic_notification)
                    .SetContentText(message);
                //.Extend(wearableExtender);

                // Obtain a reference to the NotificationManager
                var id = Sealegs.Droid.Helpers.Settings.GetUniqueNotificationId();
                Console.WriteLine("Got Unique ID: " + id);

                var notif = builder.Build();
                notif.Defaults = NotificationDefaults.All;
                Console.WriteLine("Notify");
                notificationManager.Notify(id, notif);

                dialogNotify("Sealegs Notification", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Local alert box
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        protected void dialogNotify(String title, String message)
        {

            MainActivity.Current.RunOnUiThread(() => {
                AlertDialog.Builder dlg = new AlertDialog.Builder(MainActivity.Current);
                AlertDialog alert = dlg.Create();
                alert.SetTitle(title);
                alert.SetButton("Ok", delegate
                {
                    alert.Dismiss();
                });
                alert.SetMessage(message);
                alert.Show();
            });
        }

        #endregion 

        #region OnRecoverableError / OnError

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            //Some recoverable error happened
            return true;
        }

        protected override void OnError(Context context, string errorId)
        {
            //Some more serious error happened
        }

        #endregion 
    }
}

