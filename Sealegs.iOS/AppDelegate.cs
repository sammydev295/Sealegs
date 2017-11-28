using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WindowsAzure.Messaging;
using AsNum.XFControls.iOS;
using Foundation;

using UIKit;

using FormsToolkit;
using FormsToolkit.iOS;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Plugin.Media;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using Social;
using CoreSpotlight;
using HockeyApp.iOS;
using Google.AppIndexing;
using FFImageLoading.Forms.Touch;
using Refractored.XamForms.PullToRefresh.iOS;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataStore;
using Sealegs.Clients.Portable;
using Sealegs.Clients.UI;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        #region QuickActions ShortcutIdentifier class

        /// <summary>
        /// Quick Actions registered via Info.plist
        /// </summary>
        public static class ShortcutIdentifier
        {
            public const string Tweet = "com.sealegs.lockerapp.tweet";
            public const string Announcements = "com.sealegs.lockerapp.announcements";
            public const string Events = "com.sealegs.lockerapp.events";
            public const string MiniHacks = "com.sealegs.lockerapp.minihacks";
        }

        #endregion

        #region Constants

        // Replace with your Apple ID from iTunes Connect
        public const int ITUNES_APP_ID = 1;

        #endregion 

        #region FinishedLaunching

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var tint = UIColor.FromRGB(118, 53, 235);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(250, 250, 250); //bar background
            UINavigationBar.Appearance.TintColor = tint; //Tint color of button items

            UIBarButtonItem.Appearance.TintColor = tint; //Tint color of button items

            UITabBar.Appearance.TintColor = tint;

            UISwitch.Appearance.OnTintColor = tint;

            UIAlertView.Appearance.TintColor = tint;

            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = tint;

#if !ENABLE_TEST_CLOUD

            if (!string.IsNullOrWhiteSpace(ApiKeys.HockeyAppiOS) && ApiKeys.HockeyAppiOS != nameof(ApiKeys.HockeyAppiOS))
            {
                var manager = BITHockeyManager.SharedHockeyManager;
                manager.Configure(ApiKeys.HockeyAppiOS);

                // Disable update manager
                manager.DisableUpdateManager = false;
                manager.DisableMetricsManager = false;

                manager.StartManager();
                manager.Authenticator.AuthenticateInstallation(); // This line is obsolete in crash only builds
            }

#endif

            Forms.Init();
            FormsMaps.Init();
            Toolkit.Init();
            CrossMedia.Current.Initialize();
            AsNumAssemblyHelper.HoldAssembly();
            // Image caching package
            CachedImageRenderer.Init(); // https://github.com/daniel-luberda/DLToolkit.Forms.Controls/tree/master/TagEntryView

            AppIndexing.SharedInstance.RegisterApp(618319027); // https://components.xamarin.com/gettingstarted/googleiosappindexing

            //ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            // Code for starting up the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD

            Xamarin.Calabash.Start();  // https://developer.xamarin.com/guides/testcloud/calabash/introduction-to-calabash/

            // Mapping StyleId to iOS Labels
            Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) =>
            {
                if (null != e.View.StyleId)
                {
                    e.NativeView.AccessibilityIdentifier = e.View.StyleId;
                }
            };
            
#endif

            SetMinimumBackgroundFetchInterval();

            // Random Inits for Linking out.
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            // Initialization only needed on iOS for offline Sync
            SQLitePCL.CurrentPlatform.Init();  // https://docs.microsoft.com/en-us/azure/app-service-mobile/app-service-mobile-xamarin-forms-get-started-offline-data

            // Excude for iOS
            Plugin.Share.ShareImplementation.ExcludedUIActivityTypes = new List<NSString> // https://github.com/jguertl/SharePlugin
            {
                UIActivityType.PostToFacebook,
                UIActivityType.AssignToContact,
                UIActivityType.OpenInIBooks,
                UIActivityType.PostToVimeo,
                UIActivityType.PostToFlickr,
                UIActivityType.SaveToCameraRoll
            };

            ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init(); // https://github.com/jamesmontemagno/ImageCirclePlugin
        //    ZXing.Net.Mobile.Forms.iOS.Platform.Init(); // https://components.xamarin.com/view/zxing.net.mobile

            NonScrollableListViewRenderer.Initialize();
            SelectedTabPageRenderer.Initialize();
            TextViewValue1Renderer.Init();

            PullToRefreshLayoutRenderer.Init(); // https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout

            LoadApplication(new App());

            // Process any potential notification data from launch
            ProcessNotification(options);

            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, DidBecomeActive);

            MessagingService.Current.Subscribe<string>(MessageKeys.LoggedIn, async (m, role) =>
            {
                var push = DependencyService.Get<IPushNotifications>();
                await push.RegisterForNotifications();
            });

            return base.FinishedLaunching(app, options);
            //bool alertsAllowed = false;
            //UNUserNotificationCenter.Current.GetNotificationSettings((settings) =>
            //{
            //    alertsAllowed = (settings.AlertSetting == UNNotificationSetting.Enabled);
            //    if (!alertsAllowed)
            //    {
            //        // Request notification permissions from the user
            //        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            //        {
            //            // Handle approval
            //            var push = DependencyService.Get<IPushNotifications>();
            //            push?.RegisterForNotifications();
            //        });
            //    }
            //});

           // return ret;
        }

        #endregion

        #region DidBecomeActive

        void DidBecomeActive(NSNotification notification)
        {
            ((Sealegs.Clients.UI.App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        #endregion 

        #region WillEnterForeground

        public override void WillEnterForeground(UIApplication uiApplication)
        {
            base.WillEnterForeground(uiApplication);
            ((Sealegs.Clients.UI.App)Xamarin.Forms.Application.Current).SecondOnResume();
        }

        #endregion 

        #region Remote Notifications - APNS Push Notifications

        public override void RegisteredForRemoteNotifications(UIApplication app, NSData deviceToken)
        {
#if ENABLE_TEST_CLOUD
#else
            // Register our info with Azure
            var hub = new SBNotificationHub(ApiKeys.AzureListenConnection, ApiKeys.AzureHubName);
            NSSet tags = (BaseViewModel.User?.Role != null) ? new NSSet(Sealegs.DataObjects.SealegsUserRole.RoleHierarchy.ToList().SkipWhile(r => r.Id.ToLower() != BaseViewModel.User?.Role.Id.ToLower()).Select(r => r.RoleName).ToArray()) : null;
            hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
            {
                if (errorCallback != null)
                    Console.WriteLine(errorCallback != null ? "Error: " + errorCallback.Description : "Success");
            });

            return;

            hub.UnregisterAllAsync(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("Error calling Unregister: {0}", error.ToString());
                    return;
                }

                tags = !String.IsNullOrEmpty(BaseViewModel.User?.Role?.RoleName) ? new NSSet(BaseViewModel.User?.Role?.RoleName) : null;
                hub.RegisterNativeAsync(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                        Console.WriteLine(errorCallback != null ? "Error: " + errorCallback.Description : "Success");
                });
            });
#endif
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Console.WriteLine("PushData received " + userInfo.ToString());
            ProcessNotification(userInfo);
            completionHandler(UIBackgroundFetchResult.NewData);
        }

        public override void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
        {
            // Process a notification received while the app was already open
            ProcessNotification(userInfo);
        }

        // Something went wrong while registering!
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            var alert = new UIAlertView("Computer says no", "Notification registration failed! Try again!", null, "OK", null);

            alert.Show();
        }

        void ProcessNotification(NSDictionary userInfo)
        {
            if (userInfo == null)
                return;

            Console.WriteLine("Received Notification");

            var apsKey = new NSString("aps");

            if (userInfo.ContainsKey(apsKey))
            {
                var alertKey = new NSString("alert");

                var aps = (NSDictionary)userInfo.ObjectForKey(apsKey);
                if (aps.ContainsKey(alertKey))
                {
                    var alert = (NSString)aps.ObjectForKey(alertKey);
                    try
                    {
                        var avAlert = new UIAlertView("Sealegs Update", alert, null, "OK", null);
                        avAlert.Show();
                    }
                    catch (Exception ex)
                    {

                    }

                    Console.WriteLine("Notification: " + alert);
                }
            }
        }

        #endregion

        #region Quick Actions - Can also be invoked via 3D Touch

        public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("OnActivated");

            // Handle any shortcut item being selected
            HandleShortcutItem(LaunchedShortcutItem);

            // Clear shortcut after it's been handled
            LaunchedShortcutItem = null;
        }

        // if app is already running
        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
        {
            Console.WriteLine("PerformActionForShortcutItem");

            // Perform action
            var handled = HandleShortcutItem(shortcutItem);
            completionHandler(handled);
        }

        public bool HandleShortcutItem(UIApplicationShortcutItem shortcutItem)
        {
            Console.WriteLine("HandleShortcutItem ");
            var handled = false;

            // Anything to process?
            if (shortcutItem == null)
                return false;

            // Take action based on the shortcut type
            switch (shortcutItem.Type)
            {
                case ShortcutIdentifier.Tweet:
                    Console.WriteLine("QUICKACTION: Tweet");
                    var slComposer = SLComposeViewController.FromService(SLServiceType.Twitter);
                    if (slComposer == null)
                    {
                        new UIAlertView("Unavailable", "Twitter is not available, please sign in on your devices settings screen.", null, "OK").Show();
                    }
                    else
                    {
                        slComposer.SetInitialText("#Sealegs");
                        if (slComposer.EditButtonItem != null)
                            slComposer.EditButtonItem.TintColor = UIColor.FromRGB(118, 53, 235);

                        slComposer.CompletionHandler += (result) =>
                        {
                            InvokeOnMainThread(() => UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null));
                        };

                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewControllerAsync(slComposer, true);
                    }
                    handled = true;
                    break;

                case ShortcutIdentifier.Announcements:
                    Console.WriteLine("QUICKACTION: Accouncements");
                    ContinueNavigation(AppPage.Notification);
                    handled = true;
                    break;

                case ShortcutIdentifier.MiniHacks:
                    Console.WriteLine("QUICKACTION: MiniHacks");
                    ContinueNavigation(AppPage.MiniHacks);
                    handled = true;
                    break;

                case ShortcutIdentifier.Events:
                    Console.WriteLine("QUICKACTION: Events");
                    ContinueNavigation(AppPage.Events);
                    handled = true;
                    break;
            }

            Console.Write(handled);

            // Return results
            return handled;
        }

        void ContinueNavigation(AppPage page, string id = null)
        {
            Console.WriteLine("ContinueNavigation");

            // TODO: display UI in Forms somehow
            System.Console.WriteLine("Show the page for " + page);
            MessagingService.Current.SendMessage<DeepLinkPage>("DeepLinkPage", new DeepLinkPage
            {
                Page = page,
                Id = id
            });
        }

        #endregion

        #region Background Refresh - Offline Sync

        private void SetMinimumBackgroundFetchInterval()
        {
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(MINIMUM_BACKGROUND_FETCH_INTERVAL);
        }
        // Minimum number of seconds between a background refresh this is shorter than Android because it is easily killed off.
        // 20 minutes = 20 * 60 = 1200 seconds
        private const double MINIMUM_BACKGROUND_FETCH_INTERVAL = 1200;

        // Called whenever your app performs a background fetch
        public override async void PerformFetch(UIApplication application, Action<UIBackgroundFetchResult> completionHandler)
        {
            // Do Background Fetch
            var downloadSuccessful = false;
            try
            {
                Xamarin.Forms.Forms.Init();//need for dependency services

                // Download data
                var manager = DependencyService.Get<IStoreManager>();

                downloadSuccessful = await manager.SyncAllAsync(Settings.Current.IsLoggedIn);
            }
            catch (Exception ex)
            {
                var logger = DependencyService.Get<ILogger>();
                ex.Data["Method"] = "PerformFetch";
                logger.Report(ex);
            }

            // If you don't call this, your application will be terminated by the OS.
            // Allows OS to collect stats like data cost and power consumption
            if (downloadSuccessful)
            {
                completionHandler(UIBackgroundFetchResult.NewData);
                Settings.Current.HasSyncedData = true;
                Settings.Current.LastSync = DateTime.UtcNow;
            }
            else
            {
                completionHandler(UIBackgroundFetchResult.Failed);
            }
        }

        #endregion
    }
}

