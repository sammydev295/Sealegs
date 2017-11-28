using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.AppIndexing;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Gcm;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppLinks;

using FormsToolkit;
using FormsToolkit.Droid;
using AsNum.XFControls.Droid;
using Acr.UserDialogs;
using Android.Support.V4.Hardware.Fingerprint;
using Gcm;

using Plugin.Permissions;
using Plugin.CurrentActivity;
using Plugin.Media;
using Plugin.Fingerprint;
using FFImageLoading.Forms.Droid;
using Refractored.XamForms.PullToRefresh.Droid;

using HockeyApp.Android;
using HockeyApp.Android.Metrics;

using Sealegs.Clients.Portable;
using Sealegs.Clients.UI;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

//using Gcm.Client;

namespace Sealegs.Droid
{
    [Activity(Label = "LockerApp16", Icon = "@drawable/icon", LaunchMode = LaunchMode.SingleTask, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[]
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataPathPrefix = "/locker/",
        DataHost = "lockerapp.sealegs.com")]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[]
        {
            Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "https",
        DataPathPrefix = "/locker/",
        DataHost = "lockerapp.sealegs.com")]

    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[]
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "http",
        DataHost = "lockerapp.sealegs.com")]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[]
        {
            Android.Content.Intent.CategoryDefault,
            Android.Content.Intent.CategoryBrowsable
        },
        DataScheme = "https",
        DataHost = "lockerapp.sealegs.com")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region Static Fields

        private static MainActivity current;

        public static MainActivity Current { get { return current; } }

        #endregion 

        #region Fields

        GoogleApiClient client;

        #endregion 

        #region OnCreate

        protected override void OnCreate(Bundle savedInstanceState)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(savedInstanceState);
            MainActivity.current = this;

            CrossMedia.Current.Initialize();
            Forms.Init(this, savedInstanceState);
            FormsMaps.Init(this, savedInstanceState);
            AndroidAppLinks.Init(this);
            Toolkit.Init();
            AsNumAssemblyHelper.HoldAssembly();

            // Image caching package
            CachedImageRenderer.Init();
            UserDialogs.Init(this);

            // Check and register Finger Scan if available
            CheckFingerPrint();

            PullToRefreshLayoutRenderer.Init(); https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout
            typeof(Color).GetProperty("Accent", BindingFlags.Public | BindingFlags.Static).SetValue(null, Color.FromHex("#757575"));

            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init(); // https://github.com/jamesmontemagno/ImageCirclePlugin

            //   ZXing.Net.Mobile.Forms.Android.Platform.Init(); // https://components.xamarin.com/view/zxing.net.mobile
#if ENABLE_TEST_CLOUD
            //Mapping StyleID to element content descriptions
            Xamarin.Forms.Forms.ViewInitialized += (object sender, Xamarin.Forms.ViewInitializedEventArgs e) => {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId)) {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };
#endif

#if !ENABLE_TEST_CLOUD
            InitializeHockeyApp();
#endif

            LoadApplication(new App());

            var gpsAvailable = IsPlayServicesAvailable();
            Settings.Current.PushNotificationsEnabled = gpsAvailable;

            if (gpsAvailable)
            {
                client = new GoogleApiClient.Builder(this) // http://stackoverflow.com/questions/33391731/how-to-get-googleapiclient-in-xamarin
                .AddApi(AppIndex.API)     // for app indexing support
                .Build();
            }

            MessagingService.Current.Subscribe<string>(MessageKeys.LoggedIn, async (m, role) =>
            {
                RegisterWithGCM();
            });

            OnNewIntent(Intent);

            if (!Settings.Current.PushNotificationsEnabled)
                return;
#if ENABLE_TEST_CLOUD
#else
            RegisterWithGCM();
#endif

            DataRefreshService.ScheduleRefresh(this);

            CheckForUpdates();
            //InitlizeSync();
        }

        #endregion

        #region OnPause

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterManagers();
        }

        #endregion

        #region OnDestroy

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnregisterManagers();
        }

        #endregion

        #region Hockey App

        // https://components.xamarin.com/view/hockeyappios
        void InitializeHockeyApp()
        {
            if (string.IsNullOrWhiteSpace(ApiKeys.HockeyAppAndroid) || ApiKeys.HockeyAppAndroid == nameof(ApiKeys.HockeyAppAndroid))
                return;

            CrashManager.Register(this, ApiKeys.HockeyAppAndroid);
            UpdateManager.Register(this, ApiKeys.HockeyAppAndroid);

            MetricsManager.Register(Application, ApiKeys.HockeyAppAndroid);
        }

        private void CheckForUpdates()
        {
            // Remove this for store builds!
            UpdateManager.Register(this, ApiKeys.HockeyAppAndroid);
        }

        private void UnregisterManagers()
        {
            UpdateManager.Unregister();
        }

        #endregion 

        #region Google Cloud Messaging

        /// <summary>
        /// https://developer.xamarin.com/guides/android/application_fundamentals/notifications/remote-notifications-with-gcm/
        /// </summary>
        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            System.Diagnostics.Debug.WriteLine("MainActivity", "Registering...");
            GcmService.Initialize(this);
            GcmService.Register(this);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    if (Settings.Current.GooglePlayChecked)
                        return false;

                    Settings.Current.GooglePlayChecked = true;
                    Toast.MakeText(this, "Google Play services is not installed, push notifications have been disabled.", ToastLength.Long).Show();
                }
                else
                {
                    Settings.Current.PushNotificationsEnabled = false;
                }
                return false;
            }
            else
            {
                Settings.Current.PushNotificationsEnabled = true;
                return true;
            }
        }

        #endregion

        #region Permissions Needed 

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            //  global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            // https://github.com/jamesmontemagno/PermissionsPlugin
            // http://motzcod.es/post/133939517717/simplified-ios-android-runtime-permissions-with
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion

        #region CheckFingerPrint

        public void CheckFingerPrint()
        {
            try
            {
                FingerprintManagerCompat fingerprintManager = FingerprintManagerCompat.From(this);
                KeyguardManager keyguardManager = GetSystemService(KeyguardService) as KeyguardManager;
                App.AndroidFingerPrintSupported = fingerprintManager.IsHardwareDetected && keyguardManager.IsKeyguardSecure && fingerprintManager.HasEnrolledFingerprints;
                CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);//Register FingerScan Nuget
            }
            catch (Exception e)
            {
            }


            //if (!keyguardManager.IsKeyguardSecure)
            //{
            //}
            //if (!fingerprintManager.HasEnrolledFingerprints)
            //{
            //    // Can't use fingerprint authentication - notify the user that they need to 
            //    // enroll at least one fingerprint with the device.
            //}
        }

        #endregion
    }
}

