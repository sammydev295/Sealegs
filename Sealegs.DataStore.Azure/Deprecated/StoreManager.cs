using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Newtonsoft.Json.Linq;

using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Sealegs.Clients.Portable;

namespace Sealegs.DataStore.Azure
{
    public class StoreManager : IStoreManager
    {
        #region Properties

        public static MobileServiceClient MobileService { get; set; }

        #region The Stores

        IWineStore wineStore;
        public IWineStore WineStore => wineStore ?? (wineStore = DependencyService.Get<IWineStore>());

        IWineVarietalStore wineVarietalStore;
        public IWineVarietalStore WineVarietalStore => wineVarietalStore ?? (wineVarietalStore = DependencyService.Get<IWineVarietalStore>());

        INotificationStore notificationStore;
        public INotificationStore NotificationStore => notificationStore ?? (notificationStore = DependencyService.Get<INotificationStore>());

        ICategoryStore categoryStore;
        public ICategoryStore CategoryStore => categoryStore ?? (categoryStore = DependencyService.Get<ICategoryStore>());

        IFavoriteStore favoriteStore;
        public IFavoriteStore FavoriteStore => favoriteStore ?? (favoriteStore = DependencyService.Get<IFavoriteStore>());

        IFeedbackStore feedbackStore;
        public IFeedbackStore FeedbackStore => feedbackStore ?? (feedbackStore = DependencyService.Get<IFeedbackStore>());

        ILockerMemberStore lockerStore;
        public ILockerMemberStore LockerStore => lockerStore ?? (lockerStore = DependencyService.Get<ILockerMemberStore>());

        IUserStore userStore;
        public IUserStore UserStore => userStore ?? (userStore = DependencyService.Get<IUserStore>());

        IEventStore eventStore;
        public IEventStore EventStore => eventStore ?? (eventStore = DependencyService.Get<IEventStore>());

        INewsStore newsStore;
        public INewsStore NewsStore => newsStore ?? (newsStore = DependencyService.Get<INewsStore>());

        IMiniHacksStore miniHacksStore;
        public IMiniHacksStore MiniHacksStore => miniHacksStore ?? (miniHacksStore = DependencyService.Get<IMiniHacksStore>());

        #endregion

        #endregion

        #region IStoreManager implementation

        object locker = new object();

        #region IsInitialized

        public bool IsInitialized { get; private set; }

        #endregion

        #region InitializeAsync

        public async Task InitializeAsync()
        {
            try
            {
                MobileServiceSQLiteStore store;
                lock (locker)
                {
                    if (IsInitialized)
                      return;

                    IsInitialized = true;
                    var dbId = Settings.DatabaseId;
                    var path = $"syncstore{dbId}.db";
                   
                    MobileService = new MobileServiceClient(Addresses.ApiOnline);
#if DEBUG

                    // MobileService.AlternateLoginHost = new Uri(Addresses.AccountOnline);
#endif

                    //DropEverythingAsync();

                    store = new MobileServiceSQLiteStoreWithLogging(path, true, true);

                    store.DefineTable<FeaturedEvent>();
                    store.DefineTable<Category>();
                    store.DefineTable<Favorite>();
                    store.DefineTable<Notification>();
                    store.DefineTable<Feedback>();
                    store.DefineTable<LockerMember>();
                    store.DefineTable<SealegsUser>();
                    store.DefineTable<News>();
                    store.DefineTable<Wine>();
                    store.DefineTable<WineVarietal>();
                    store.DefineTable<StoreSettings>();
                }

                await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
                await LoadCachedTokenAsync();

                //await SyncAllAsync(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to InitializeAsync: " + ex);
            }
        }

        #endregion 

        #region LoginAsync

        public async Task<MobileServiceUser> LoginAsync(string username, string password)
        {
            MobileService = new MobileServiceClient(Addresses.ApiOnline);
            var credentials = new JObject
            {
                ["email"] = username,
                ["password"] = password
            };
            try
            {
                MobileServiceUser user = await MobileService.LoginAsync("SealegsAuth", credentials);
                await InitializeAsync();
                await CacheToken(user);
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to login: " + ex);
            }

            return null;
        }

        #endregion

        #region LogoutAsync

        public async Task LogoutAsync()
        {
            if (!IsInitialized)
            {
                await InitializeAsync();
            }

            await MobileService.LogoutAsync();

            StoreSettings settings = null;
            try
            {
                settings = await ReadSettingsAsync();
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to read settings items: " + ex);
            }

            if (settings != null)
            {
                settings.AuthToken = string.Empty;
                settings.UserId = string.Empty;

                await SaveSettingsAsync(settings);
            }
        }

        #endregion

        #region SaveSettingsAsync

        async Task SaveSettingsAsync(StoreSettings settings) =>
            await MobileService.SyncContext.Store.UpsertAsync(nameof(StoreSettings), new[] { JObject.FromObject(settings) }, true);

        #endregion

        #region ReadSettingsAsync

        async Task<StoreSettings> ReadSettingsAsync() =>
            (await MobileService.SyncContext.Store.LookupAsync(nameof(StoreSettings), StoreSettings.StoreSettingsId))?.ToObject<StoreSettings>();

        #endregion

        #region CacheToken

        async Task CacheToken(MobileServiceUser user)
        {
            var claims = JwtUtility.GetClaims(user.MobileServiceAuthenticationToken);
            var settings = new StoreSettings
            {
                UserId = claims[JwtClaimNames.Subject],
                Email = claims["email"],
                FirstName = claims[JwtClaimNames.GivenName],
                LastName = claims[JwtClaimNames.FamilyName],
                RoleName = claims["role"],
                AuthToken = user.MobileServiceAuthenticationToken
            };
           
            await SaveSettingsAsync(settings);
        }

        #endregion

        #region LoadCachedTokenAsync

        async Task LoadCachedTokenAsync()
        {
            StoreSettings settings = await ReadSettingsAsync();
            if (settings != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(settings.AuthToken) && JwtUtility.GetTokenExpiration(settings.AuthToken) > DateTime.UtcNow)
                    {
                        MobileService.CurrentUser =
                            new MobileServiceUser(settings.UserId)
                            {
                                MobileServiceAuthenticationToken = settings.AuthToken
                            };
                    }
                }
                catch (InvalidTokenException)
                {
                    settings.AuthToken = string.Empty;
                    settings.UserId = string.Empty;

                    await SaveSettingsAsync(settings);
                }
            }
        }

        #endregion

        #region SyncAllAsync

        /// <summary>
        /// Syncs all tables.
        /// </summary>
        /// <returns>The all async.</returns>
        /// <param name="syncUserSpecific">If set to <c>true</c> sync user specific.</param>
        public async Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            if (!IsInitialized)
                await InitializeAsync();

            var taskList = new List<Task<bool>>();
            taskList.Add(LockerStore.SyncAsync());
            taskList.Add(CategoryStore.SyncAsync());
            taskList.Add(EventStore.SyncAsync());
            taskList.Add(UserStore.SyncAsync());
            taskList.Add(NotificationStore.SyncAsync());
            taskList.Add(WineStore.SyncAsync());
            taskList.Add(NewsStore.SyncAsync());
            taskList.Add(WineVarietalStore.SyncAsync());

            if (syncUserSpecific)
            {
                taskList.Add(FeedbackStore.SyncAsync());
                taskList.Add(FavoriteStore.SyncAsync());
            }

            var successes = await Task.WhenAll(taskList);
            return successes.Any(x => !x);//if any were a failure.
        }

        #endregion 

        #region DropEverythingAsync

        /// <summary>
        /// Drops all tables from the database and updated DB Id
        /// </summary>
        /// <returns>The everything async.</returns>
        public Task DropEverythingAsync()
        {
            Settings.UpdateDatabaseId();
            CategoryStore.DropTable();
            EventStore.DropTable();
            MiniHacksStore.DropTable();
            NotificationStore.DropTable();
            LockerStore.DropTable();
            WineStore.DropTable();
            UserStore.DropTable();
            NewsStore.DropTable();
            FeedbackStore.DropTable();
            FavoriteStore.DropTable();
            WineVarietalStore.DropTable();

            IsInitialized = false;
            return Task.FromResult(true);
        }

        #endregion

        #endregion
    }

    public class MobileServiceSQLiteStoreWithLogging : MobileServiceSQLiteStore
    {
        private bool logResults;
        private bool logParameters;

        public MobileServiceSQLiteStoreWithLogging(string fileName, bool logResults = false, bool logParameters = false)
            : base(fileName)
        {
            this.logResults = logResults;
            this.logParameters = logParameters;
        }

        protected override IList<Newtonsoft.Json.Linq.JObject> ExecuteQueryInternal(string tableName, string sql, IDictionary<string, object> parameters)
        {
            Debug.WriteLine(sql);

            if (logParameters)
                PrintDictionary(parameters);

            var result = base.ExecuteQueryInternal(tableName, sql, parameters);

            if (logResults && result != null)
            {
                foreach (var token in result)
                    Debug.WriteLine(token);
            }

            return result;
        }

        protected override void ExecuteNonQueryInternal(string sql, IDictionary<string, object> parameters)
        {
            Debug.WriteLine(sql);

            if (logParameters)
                PrintDictionary(parameters);

            base.ExecuteNonQueryInternal(sql, parameters);
        }

        private void PrintDictionary(IDictionary<string, object> dictionary)
        {
            if (dictionary == null)
                return;

            foreach (var pair in dictionary)
                Debug.WriteLine($"{pair.Key}:{pair.Value}");
        }
    }

    public class LoggingHandler : DelegatingHandler
    {
        private bool logRequestResponseBody;

        public LoggingHandler(bool logRequestResponseBody = false)
        {
            this.logRequestResponseBody = logRequestResponseBody;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Request: {request.Method} {request.RequestUri.ToString()}");

            if (logRequestResponseBody && request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync();
                Debug.WriteLine(requestContent);
            }

            var response = await base.SendAsync(request, cancellationToken);

            Debug.WriteLine($"Response: {response.StatusCode}");

            if (logRequestResponseBody)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseContent);
            }

            return response;
        }
    }

    #region StoreSettings Class

    public class StoreSettings
    {
        #region Fields

        public const string StoreSettingsId = "store_settings";

        #endregion

        #region CTOR

        public StoreSettings()
        {
            Id = StoreSettingsId;
        }


        #endregion

        #region Properties

        public string Id { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string RoleName { get; set; }

        public string AuthToken { get; set; }

        #endregion
    }

    #endregion
}



