using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using FormsToolkit;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions.SQLite;
using Sealegs.DataStore.Mock.Tables;
using Sealegs.Utils;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.NavigationService;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Clients.UI.Pages.Wine;
using Sealegs.Clients.UI.Pages.News;
using Sealegs.Clients.UI.Pages.Events;
using Sealegs.Clients.UI.Pages.Android.Menu;
using Sealegs.Clients.UI.Pages.ChillRequest;
using Sealegs.Clients.UI.Pages.PopUp;
using Sealegs.Clients.UI.Pages.Profile;
using Sealegs.Clients.UI.Pages.Support;
using Sealegs.Clients.UI.Pages.Evaluation;
using Sealegs.Clients.UI.Test;

namespace Sealegs.Clients.UI
{
    public partial class App : Application
    {
        #region Static Fields

        public static App current;

        public static LockerMember LockerMember;

        #endregion

        #region Static AndroidFingerPrintSupported

        static bool _androidFingerPrintSupported = false;
        public static bool AndroidFingerPrintSupported
        {
            get
            {
                return _androidFingerPrintSupported;
            }
            set
            {
                _androidFingerPrintSupported = value;
            }
        }

        #endregion

        #region Locator

        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());

        #endregion

        #region Logger

        static ILogger _logger;
        public static ILogger Logger => _logger ?? (_logger = DependencyService.Get<ILogger>());

        #endregion

        #region CTOR

        public App()
        {
            current = this;
            InitializeComponent();

            var nav = new NavigationService();

            #region Navigation Pages

            nav.Configure(ViewModelLocator.LoginPage, typeof(LoginPage));
            nav.Configure(ViewModelLocator.Master, typeof(Master));
            nav.Configure(ViewModelLocator.Locker, typeof(LockersPage));
            nav.Configure(ViewModelLocator.LockerDetails, typeof(LockerDetailsPage));

            nav.Configure(ViewModelLocator.AddEditEventPage, typeof(AddEditEventPage));
            nav.Configure(ViewModelLocator.AddEditNews, typeof(AddEditNewspage));
            nav.Configure(ViewModelLocator.AddEditNotifications, typeof(AddEditNotificationPage));
            nav.Configure(ViewModelLocator.AddEmployee, typeof(AddEmployee));
            nav.Configure(ViewModelLocator.AddWine, typeof(AddWinePage));
            nav.Configure(ViewModelLocator.CheckOutFinal, typeof(CheckOutFinal));
            nav.Configure(ViewModelLocator.CheckOut, typeof(CheckOutPage));
            nav.Configure(ViewModelLocator.Employees, typeof(Employees));
            nav.Configure(ViewModelLocator.Evaluations, typeof(EvaluationsPage));
            nav.Configure(ViewModelLocator.EventDetails, typeof(EventDetailsPage));
            nav.Configure(ViewModelLocator.Events, typeof(EventsPage));
            nav.Configure(ViewModelLocator.Feedback, typeof(FeedbackPage));
            nav.Configure(ViewModelLocator.Feed, typeof(FeedPage));
            nav.Configure(ViewModelLocator.NewsDetails, typeof(NewsDetailsPage));
            nav.Configure(ViewModelLocator.News, typeof(News));
            nav.Configure(ViewModelLocator.NewsManagment, typeof(NewsManagmentPage));

            nav.Configure(ViewModelLocator.NotificationManagment, typeof(NotificationsManagmentPage));
            nav.Configure(ViewModelLocator.Settings, typeof(SettingsPage));
            nav.Configure(ViewModelLocator.Signature, typeof(Signature));
            nav.Configure(ViewModelLocator.WineCarousel, typeof(WinesCarouselPage));
            nav.Configure(ViewModelLocator.WineCardView, typeof(WinesCardViewPage));
            nav.Configure(ViewModelLocator.WineDetail, typeof(WineDetailsPopUp));
            nav.Configure(ViewModelLocator.ContactUs, typeof(ContactUs));
            nav.Configure(ViewModelLocator.Profile, typeof(Profile));
            nav.Configure(ViewModelLocator.AddEditLocker, typeof(AddEditLockerPage));
            nav.Configure(ViewModelLocator.AboutUs, typeof(AboutPage));
            nav.Configure(ViewModelLocator.Wine, typeof(Wines));
            nav.Configure(ViewModelLocator.EventsManagment, typeof(EventsManagementPage));
            nav.Configure(ViewModelLocator.RemoteChillRequest, typeof(RemoteChillRequestPage));
            nav.Configure(ViewModelLocator.RatingBarPopUp, typeof(RatingBarPopUp));
            nav.Configure(ViewModelLocator.WineEvaluation, typeof(WineEvaluation));

            #endregion

            // Parameter true means mock data
            ViewModelBase.Init(false);

            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
                SimpleIoc.Default.Register<INavigationService>(() => nav);
            var firstPage = GetMainPage();
            nav.Initialize(firstPage);

            MainPage = firstPage; // The root page of your application

            Task.Run(() =>
            {
                FFImageLoading.ImageService.Instance.Initialize();
#if DEBUG
                FFImageLoading.ImageService.Instance.Config.Logger = new CustomMiniLogger();
                FFImageLoading.ImageService.Instance.Config.VerboseLoadingCancelledLogging = true;
                FFImageLoading.ImageService.Instance.Config.VerboseLogging = true;
                FFImageLoading.ImageService.Instance.Config.VerboseMemoryCacheLogging = true;
                FFImageLoading.ImageService.Instance.Config.VerbosePerformanceLogging = true;
#endif       
            });
        }
        #endregion

        #region GetMainPage

        private NavigationPage GetMainPage()
        {
            return new SealegsNavigationPage(new LoginPage());

            //if (Device.OS == TargetPlatform.Android && AndroidFingerPrintSupported)
            //{
            //    return new SealegsNavigationPage(new FingerPrintScan(user));
            //}
            //return new SealegsNavigationPage(new FingerPrintScan(user));

            //switch (Device.OS)
            //{
            //    case TargetPlatform.Android:
            //        // MainPage = new RootPageAndroid();
            //        return new SealegsNavigationPage(new LoginPage());
            //        break;
            //    case TargetPlatform.iOS:
            //        return new SealegsNavigationPage(new RootPageiOS());
            //        break;

            //    default:
            //        throw new NotImplementedException();
            //}
        }

        #endregion

        #region Event Overrides

        protected override void OnStart()
        {
            OnResume();
        }

        public void SecondOnResume()
        {
            OnResume();
        }

        bool registered;
        bool firstRun = true;

        protected override void OnResume()
        {
            if (registered)
                return;

            registered = true;

            // Handle when your app resumes
            Settings.Current.IsConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;

            // Handle when your app starts
            MessagingService.Current.Subscribe<MessagingServiceAlert>(MessageKeys.Message, async (m, info) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);
                if (task == null)
                    return;

                await task;
                info?.OnCompleted?.Invoke();
            });

            MessagingService.Current.Subscribe<MessagingServiceAlert>(MessageKeys.Error, async (m, info) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(info.Title, info.Message, info.Cancel);
                if (task == null)
                    return;

                await task;
                info?.OnCompleted?.Invoke();
            });

            MessagingService.Current.Subscribe<MessagingServiceQuestion>(MessageKeys.Question, async (m, q) =>
            {
                var task = Application.Current?.MainPage?.DisplayAlert(q.Title, q.Question, q.Positive, q.Negative);
                if (task == null)
                    return;

                var result = await task;
                q?.OnCompleted?.Invoke(result);
            });

            MessagingService.Current.Subscribe<MessagingServiceChoice>(MessageKeys.Choice, async (m, q) =>
            {
                var task = Application.Current?.MainPage?.DisplayActionSheet(q.Title, q.Cancel, q.Destruction, q.Items);
                if (task == null)
                    return;

                var result = await task;
                q?.OnCompleted?.Invoke(result);
            });

            MessagingService.Current.Subscribe(MessageKeys.NavigateLogin, m =>
            {
                var nav = DependencyService.Get<INavigationService>();
                nav.NavigateTo(ViewModelLocator.LoginPage);
            });

            MessagingService.Current.Subscribe(MessageKeys.NavigateToLocker, m =>
            {
                var nav = DependencyService.Get<INavigationService>();
                nav.NavigateTo(ViewModelLocator.Locker);
            });

            try
            {
                if (firstRun || Device.OS != TargetPlatform.iOS)
                    return;

                var mainNav = MainPage as NavigationPage;
                if (mainNav == null)
                    return;

                var rootPage = mainNav.CurrentPage as RootPageiOS;
                if (rootPage == null)
                    return;

                var rootNav = rootPage.CurrentPage as NavigationPage;
                if (rootNav == null)
                    return;

                var about = rootNav.CurrentPage as AboutPage;
                if (about != null)
                {
                    about.OnResume();
                    return;
                }

                var lockers = rootNav.CurrentPage as LockersPage;
                if (lockers != null)
                {
                    //  lockers.OnResume();
                    return;
                }

                var feed = rootNav.CurrentPage as FeedPage;
                if (feed != null)
                {
                    feed.OnResume();
                    return;
                }
            }
            catch
            {
            }
            finally
            {
                firstRun = false;
            }
        }

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            var data = uri.ToString().ToLowerInvariant();
            //only if deep linking
            if (!data.Contains("/locker/"))
                return;

            var id = data.Substring(data.LastIndexOf("/", StringComparison.Ordinal) + 1);

            if (!string.IsNullOrWhiteSpace(id))
            {
                MessagingService.Current.SendMessage<DeepLinkPage>("DeepLinkPage", new DeepLinkPage
                {
                    Page = AppPage.Lockers,
                    Id = id
                });
            }

            base.OnAppLinkRequestReceived(uri);
        }


        protected override void OnSleep()
        {
            if (!registered)
                return;

            registered = false;
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
            MessagingService.Current.Unsubscribe<MessagingServiceQuestion>(MessageKeys.Question);
            MessagingService.Current.Unsubscribe<MessagingServiceAlert>(MessageKeys.Message);
            MessagingService.Current.Unsubscribe<MessagingServiceChoice>(MessageKeys.Choice);

            // Handle when your app sleeps

            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        }

        protected async void ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //save current state and then set it
            var connected = Settings.Current.IsConnected;
            Settings.Current.IsConnected = e.IsConnected;
            if (connected && !e.IsConnected)
            {
                //we went offline, should alert the user and also update ui (done via settings)
                var task = Application.Current?.MainPage?.DisplayAlert("Offline", "Uh Oh, It looks like you have gone offline. Please check your internet connection to get the latest data and enable syncing data.", "OK");
                if (task != null)
                    await task;
            }
        }

        #endregion

        #region CustomMiniLogger 

        public class CustomMiniLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                System.Diagnostics.Debug.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(errorMessage + ex.ToString());
            }
        }

        #endregion 
    }
}

