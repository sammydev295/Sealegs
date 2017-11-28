using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

using Plugin.Share;
using Xamarin.Forms;
using Plugin.Share.Abstractions;

using Sealegs.Clients.Portable.Interfaces;
using Sealegs.Utils;
using Sealegs.Clients.Portable.NavigationService;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Sealegs.DataStore.Abstractions.REST;
using Sealegs.DataStore.Abstractions.SQLite;
using Sealegs.DataStore.Mock.Tables;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class BaseViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        #region Fields

        protected static AccountResponse  Response;

        #endregion

        #region Properties

        #region ButtonName

        private string _buttonName;
        public string ButtonName
        {
            get => _buttonName;
            set
            {
                _buttonName = value;
                RaisePropertyChanged();
            }
        }

        #endregion 

        #region PageTitle

        private string _pageTitle;
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region ListVisibilty

        private bool _listVisibilty;
        public bool ListVisibilty
        {
            get => _listVisibilty;
            set
            {
                _listVisibilty = value;
                RaisePropertyChanged();
            }
        }

        #endregion 

        #region IsBusy

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsListBusy

        private bool _isListBusy;
        public bool IsListBusy
        {
            get => _isListBusy;
            set
            {
                _isListBusy = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region IsNotBusy

        public bool IsNotBusy
        {
            get { return !isBusy; }
        }

        #endregion

        #endregion

        #region CTOR

        public BaseViewModel()
        {

        }

        static BaseViewModel()
        {
            Init();
        }

        #endregion

        #region Init

        public static void Init()
        {
            DependencyService.Register<IAuth, DataStore.Azure.Api.Auth>();
            DependencyService.Register<INews, DataStore.Azure.Api.News>();
            DependencyService.Register<IFeaturedEvents, DataStore.Azure.Api.FeaturedEvents>();
            DependencyService.Register<IWines, DataStore.Azure.Api.Wines>();
            DependencyService.Register<ILockerMember, DataStore.Azure.Api.LockerMember>();
            DependencyService.Register<IRemoteChilledRequest, DataStore.Azure.Api.RemoteChilledRequest>();
            DependencyService.Register<IWineVarietal, DataStore.Azure.Api.WineVarietal>();
            DependencyService.Register<ILockerType, DataStore.Azure.Api.LockerTypes>();
            DependencyService.Register<INotifications, DataStore.Azure.Api.Notifications>();
            DependencyService.Register<IRole, DataStore.Azure.Api.Role>();
            DependencyService.Register<IFavorite, DataStore.Azure.Api.Favorite>();
            DependencyService.Register<IFeedback, DataStore.Azure.Api.Feedback>();

            DependencyService.Register<IUserTable, UserTable>();
            DependencyService.Register<ILockerFilterSetting, LockerFilterSettingTable>();

#if ENABLE_TEST_CLOUD && !DEBUG
            DependencyService.Register<IWineStore, Sealegs.DataStore.Mock.WineStore>();
            DependencyService.Register<IWineVarietalStore, Sealegs.DataStore.Mock.WineVarietalStore>();
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
            DependencyService.Register<IWineStore, Sealegs.DataStore.Azure.WineStore>();
            DependencyService.Register<IWineVarietalStore, Sealegs.DataStore.Azure.WineVarietalStore>();
            DependencyService.Register<ILockerMemberStore, Sealegs.DataStore.Azure.LockerMemberStore>();
            DependencyService.Register<IFavoriteStore, Sealegs.DataStore.Azure.FavoriteStore>();
            DependencyService.Register<IFeedbackStore, Sealegs.DataStore.Azure.FeedbackStore>();
            DependencyService.Register<IUserStore, Sealegs.DataStore.Azure.UserStore>();
            DependencyService.Register<INewsStore, Sealegs.DataStore.Azure.NewsStore>();
            DependencyService.Register<ICategoryStore, Sealegs.DataStore.Azure.CategoryStore>();
            DependencyService.Register<IEventStore, Sealegs.DataStore.Azure.EventStore>();
            DependencyService.Register<INotificationStore, Sealegs.DataStore.Azure.NotificationStore>();
            DependencyService.Register<IMiniHacksStore, Sealegs.DataStore.Azure.MiniHacksStore>();
            DependencyService.Register<ISSOClient, Sealegs.Clients.Portable.Auth.Azure.XamarinSSOClient>();
            DependencyService.Register<IStoreManager, Sealegs.DataStore.Azure.StoreManager>();
#endif
            DependencyService.Register<IAzureBlobStorage, AzureBlobStorage>();
            DependencyService.Register<MediaService>();
            DependencyService.Register<IAzureCognitiveServices, AzureCognitiveServices>();
            DependencyService.Register<Sealegs.Clients.Portable.IFavoriteService, Sealegs.Clients.Portable.FavoriteService>();

            // Set content paths
            DependencyService.Get<ILockerMemberStore>().ImagesURI = Addresses.LockersImagesDirectAddress;
            DependencyService.Get<IWineStore>().ImagesURI = Addresses.WinesImagesDirectAddress;
            DependencyService.Get<IUserStore>().ImagesURI = Addresses.ResourcesImagesDirectAddress;
        }

        #endregion

        #region REST API

        public static INews NewsDb => DependencyService.Get<INews>();
        public static IFeaturedEvents FeaturedEventsDb => DependencyService.Get<IFeaturedEvents>();
        public static ILockerMember LockerMemberDb => DependencyService.Get<ILockerMember>();
        public static ILockerType LockerTypeDb => DependencyService.Get<ILockerType>();
        public static IWines WinesDb => DependencyService.Get<IWines>();
        public static IRemoteChilledRequest RemoteChilledRequestDb => DependencyService.Get<IRemoteChilledRequest>();
        public static IWineVarietal WineVarietalDb => DependencyService.Get<IWineVarietal>();
        public static INotifications NotificationsDb => DependencyService.Get<INotifications>();
        public static IRole RoleDb => DependencyService.Get<IRole>();
        public static IFavorite FavoriteDb => DependencyService.Get<IFavorite>();
        public static IFeedback FeedbackDb => DependencyService.Get<IFeedback>();

        #endregion

        #region SQLite
        public static IUserTable UserDb => DependencyService.Get<IUserTable>();
        public static ILockerFilterSetting LockerFilterSettingDb => DependencyService.Get<ILockerFilterSetting>();

        #endregion

        #region Dependency Services

        protected static ILogger Logger { get; } = DependencyService.Get<ILogger>();

        protected static IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();

        protected static IFavoriteStore FavoriteStore { get; } = DependencyService.Get<IFavoriteStore>();

        protected static ILockerMemberStore LockerMemberStore { get; } = DependencyService.Get<ILockerMemberStore>();

        protected static IWineStore WineStore { get; } = DependencyService.Get<IWineStore>();

        protected static IWineVarietalStore WineVarietalStore { get; } = DependencyService.Get<IWineVarietalStore>();

        protected static IEventStore EventStore { get; } = DependencyService.Get<IEventStore>();

        protected static INewsStore NewsStore { get; } = DependencyService.Get<INewsStore>();

        protected static IFeedbackStore FeedbackStore { get; } = DependencyService.Get<IFeedbackStore>();

        protected static INotificationStore NotificationStore { get; } = DependencyService.Get<INotificationStore>();

        protected static IUserStore UserStore { get; } = DependencyService.Get<IUserStore>();

        protected static IToast Toast { get; } = DependencyService.Get<IToast>();

        protected static IFavoriteService FavoriteService { get; } = DependencyService.Get<IFavoriteService>();

        public static IUserTable UserTable { get; } = DependencyService.Get<IUserTable>();

        public static Settings Settings => Settings.Current;

        public static SealegsUser User { get; set; }

        protected static IAzureBlobStorage Blob { get; } = DependencyService.Get<IAzureBlobStorage>();

        protected static MediaService Media { get; } = DependencyService.Get<MediaService>();

        protected static IAzureCognitiveServices AI { get; } = DependencyService.Get<IAzureCognitiveServices>();

        #endregion

        #region LaunchBrowserCommand

        ICommand _launchBrowserCommand;
        public ICommand LaunchBrowserCommand =>
            _launchBrowserCommand ?? (_launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

        async Task ExecuteLaunchBrowserAsync(string arg)
        {
            //if (IsBusy)
            //    return;

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

        #region Observable Properties

        public  LockerMember LockerMemberUser { get; set; }

        #endregion
    }

    #region Class SeaLegsGrouping<K, T> 

    public class SeaLegsGrouping<K, T> : ObservableCollection<T>
    {
        public K MenuKey { get; set; }
        public IList<T> MenuItems
        {
            get => base.Items;
            set
            {
                base.Items.Clear();
                foreach (var item in value)
                    base.Items.Add(item);
            }
        }

        public SeaLegsGrouping()
        {
        }

        public SeaLegsGrouping(K key, IEnumerable<T> items)
        {
            MenuKey = key;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }

    #endregion 
}
