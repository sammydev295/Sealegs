using System;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using MvvmHelpers;

using Plugin.Share;
using Plugin.Share.Abstractions;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable
{
    public class ViewModelBase : BaseViewModel
    {
        protected INavigation Navigation { get; }

        #region CTOR

        public ViewModelBase(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        static ViewModelBase()
        {
        }

        #endregion

        #region Init

        public static void Init(bool mock = true)
        {

#if ENABLE_TEST_CLOUD && !DEBUG
                DependencyService.Register<ILockerMemberStore, Sealegs.DataStore.Mock.LockerStore>();
                DependencyService.Register<IFavoriteStore, Sealegs.DataStore.Mock.FavoriteStore>();
                DependencyService.Register<IFeedbackStore, Sealegs.DataStore.Mock.FeedbackStore>();
                DependencyService.Register<ISpeakerStore, Sealegs.DataStore.Mock.SpeakerStore>();
                DependencyService.Register<INewsStore, Sealegs.DataStore.Mock.NewsStore>();
                DependencyService.Register<ICategoryStore, Sealegs.DataStore.Mock.CategoryStore>();
                DependencyService.Register<IEventStore, Sealegs.DataStore.Mock.EventStore>();
                DependencyService.Register<INotificationStore, Sealegs.DataStore.Mock.NotificationStore>();
                DependencyService.Register<IMiniHacksStore, Sealegs.DataStore.Mock.MiniHacksStore>();
                DependencyService.Register<ISSOClient, Sealegs.Clients.Portable.Auth.XamarinSSOClient>();
                DependencyService.Register<IStoreManager, Sealegs.DataStore.Mock.StoreManager>();
#else
          
                DependencyService.Register<IUserStore, DataStore.Azure.UserStore>();
            //DependencyService.Register<IWineStore, Sealegs.DataStore.Azure.WineStore>();
            //DependencyService.Register<ILockerMemberStore, Sealegs.DataStore.Azure.LockerMemberStore>();
            //DependencyService.Register<IFavoriteStore, Sealegs.DataStore.Azure.FavoriteStore>();
            //DependencyService.Register<IFeedbackStore, Sealegs.DataStore.Azure.FeedbackStore>();
            //DependencyService.Register<IUserStore, Sealegs.DataStore.Azure.UserStore>();
            //DependencyService.Register<INewsStore, Sealegs.DataStore.Azure.NewsStore>();
            //DependencyService.Register<ICategoryStore, Sealegs.DataStore.Azure.CategoryStore>();
            //DependencyService.Register<IEventStore, Sealegs.DataStore.Azure.EventStore>();
            //DependencyService.Register<INotificationStore, Sealegs.DataStore.Azure.NotificationStore>();
            //DependencyService.Register<IMiniHacksStore, Sealegs.DataStore.Azure.MiniHacksStore>();
            //DependencyService.Register<ISSOClient, Sealegs.Clients.Portable.Auth.Azure.XamarinSSOClient>();
            //DependencyService.Register<IStoreManager, Sealegs.DataStore.Azure.StoreManager>();


#endif
            DependencyService.Register<IAzureBlobStorage, AzureBlobStorage>();
            DependencyService.Register<MediaService>();
            DependencyService.Register<IAzureCognitiveServices, AzureCognitiveServices>();
            DependencyService.Register<Sealegs.Clients.Portable.IFavoriteService, Sealegs.Clients.Portable.FavoriteService>();
            DependencyService.Register<Sealegs.DataStore.Abstractions.SQLite.IUserTable, Sealegs.DataStore.Mock.Tables.UserTable>();

            // Set content paths
            //DependencyService.Get<ILockerMemberStore>().ImagesURI = Addresses.LockersImagesDirectAddress;
            //DependencyService.Get<IWineStore>().ImagesURI = Addresses.WinesImagesDirectAddress;
            DependencyService.Get<IUserStore>().ImagesURI = Addresses.ResourcesImagesDirectAddress;
        }

        #endregion

        #region Dependency Services

        protected static ILogger Logger { get; } = DependencyService.Get<ILogger>();

        protected static IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();

        protected static IFavoriteStore FavoriteStore { get; } = DependencyService.Get<IFavoriteStore>();

        protected static ILockerMemberStore LockerMemberStore { get; } = DependencyService.Get<ILockerMemberStore>();

        protected static IWineStore WineStore { get; } = DependencyService.Get<IWineStore>();

        protected static IEventStore EventStore { get; } = DependencyService.Get<IEventStore>();

        protected static INewsStore NewsStore { get; } = DependencyService.Get<INewsStore>();

        protected static IFeedbackStore FeedbackStore { get; } = DependencyService.Get<IFeedbackStore>();

        protected static INotificationStore NotificationStore { get; } = DependencyService.Get<INotificationStore>();

        protected static IUserStore UserStore { get; } = DependencyService.Get<IUserStore>();

        protected static IToast Toast { get; } = DependencyService.Get<IToast>();

        protected static IFavoriteService FavoriteService { get; } = DependencyService.Get<IFavoriteService>();

        public static Settings Settings
        {
            get { return Settings.Current; }
        }

        public static SealegsUser User { get; set; }

        #endregion

        #region LaunchBrowserCommand

        ICommand launchBrowserCommand;
        public ICommand LaunchBrowserCommand =>
        launchBrowserCommand ?? (launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

        async Task ExecuteLaunchBrowserAsync(string arg)
        {
            if (IsBusy)
                return;

            if (!arg.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                arg = "http://" + arg;

            Logger.Track(SealegsLoggerKeys.LaunchedBrowser, "Url", arg);

            var lower = arg.ToLowerInvariant();
            if (Device.OS == TargetPlatform.iOS && lower.Contains("twitter.com"))
            {
                try
                {
                    var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
                    var launchTwitter = DependencyService.Get<ILaunchTwitter>();
                    if (lower.Contains("/status/"))
                    {
                        //status
                        if (launchTwitter.OpenStatus(id))
                            return;
                    }
                    else
                    {
                        //user
                        if (launchTwitter.OpenUserName(id))
                            return;
                    }
                }
                catch
                {
                }
            }

            try
            {
                await CrossShare.Current.OpenBrowser(arg, new BrowserOptions
                {
                    ChromeShowTitle = true,
                    ChromeToolbarColor = new ShareColor
                    {
                        A = 255,
                        R = 118,
                        G = 53,
                        B = 235
                    },
                    //UseSafairReaderMode = true,
                    UseSafariWebViewController = true
                });
            }
            catch
            {
            }
        }

        #endregion
    }
}


