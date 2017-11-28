using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

using MvvmHelpers;
using Xamarin.Forms;
using FormsToolkit;

using PCLStorage;
using Plugin.EmbeddedResource;

using Newtonsoft.Json;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public class FeedViewModel : BaseViewModel
    {
        #region Fields

        public ObservableRangeCollection<Tweet> Tweets { get; } = new ObservableRangeCollection<Tweet>();

        public DateTime NextForceRefresh { get; set; }

        #endregion

        #region CTOR

        public FeedViewModel()
        {
            NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
        }

        #endregion
        
        #region Refresh Command

        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommandAsync())); 

        async Task ExecuteRefreshCommandAsync()
        {
            try
            {
                NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
                IsBusy = true;
                var tasks = new[]
                    {
                        ExecuteLoadNotificationsCommandAsync(),
                        ExecuteLoadSocialCommandAsync(),
                    };

                await Task.WhenAll(tasks);
            }
            catch(Exception ex)
            {
                ex.Data["method"] = "ExecuteRefreshCommandAsync";
                Logger.Report(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Notifications 

        Notification notification;
        public Notification Notification
        {
            get { return notification; }
            set
            {
                notification=value;
                RaisePropertyChanged();
            }
        }

        bool loadingNotifications;
        public bool LoadingNotifications
        {
            get { return loadingNotifications; }
            set {
                loadingNotifications = value;
                RaisePropertyChanged();
            }
        }

        ICommand  loadNotificationsCommand;
        public ICommand LoadNotificationsCommand =>
            loadNotificationsCommand ?? (loadNotificationsCommand = new Command(async () => await ExecuteLoadNotificationsCommandAsync())); 

        async Task ExecuteLoadNotificationsCommandAsync()
        {
            if (LoadingNotifications)
                return;

            LoadingNotifications = true;

            #if DEBUG
            await Task.Delay(1000);
            #endif

            try
            {
                var abc = await StoreManager.NewsStore.GetLatestNews();
            }
            catch(Exception ex)
            {
                ex.Data["method"] = "ExecuteLoadNotificationsCommandAsync";
                Logger.Report(ex);
                Notification = new Notification
                    {
                        Date = DateTime.UtcNow,
                        Text = "Welcome to the PRJKT Group!"
                    };   
            }
            finally
            {
                LoadingNotifications = false;
            }
        }

        #endregion

        #region Tweets

        bool loadingSocial;
        public bool LoadingSocial
        {
            get { return loadingSocial; }
            set {
                loadingSocial = value;
                RaisePropertyChanged();
            }
        }


        ICommand  loadSocialCommand;
        public ICommand LoadSocialCommand =>
            loadSocialCommand ?? (loadSocialCommand = new Command(async () => await ExecuteLoadSocialCommandAsync())); 

        async Task ExecuteLoadSocialCommandAsync()
        {
            if (LoadingSocial)
                return;

            LoadingSocial = true;
            try
            {
                SocialError = false;
                Tweets.Clear();
               
                using(var client = new HttpClient())
                {
                    #if ENABLE_TEST_CLOUD
                                        var json = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("Sealegs.Clients.Portable")), "sampletweets.txt");
                                        Tweets.ReplaceRange(JsonConvert.DeserializeObject<List<Tweet>>(json));
                    #else
                    var manager = DependencyService.Get<IStoreManager>() as Sealegs.DataStore.Azure.StoreManager;
                    if (manager == null)
                        return;

                    await manager.InitializeAsync ();

                    var mobileClient = Sealegs.DataStore.Azure.StoreManager.MobileService;
                    if (mobileClient == null)
                        return;
                    
                    var json =  await mobileClient.InvokeApiAsync<string> ("Tweet", System.Net.Http.HttpMethod.Get, null);

                    if (string.IsNullOrWhiteSpace(json)) 
                    {
                        SocialError = true;
                        return;
                    }

                    Tweets.ReplaceRange(JsonConvert.DeserializeObject<List<Tweet>>(json));
                    #endif
                }

            }
            catch (Exception ex)
            {
                SocialError = true;
                ex.Data["method"] = "ExecuteLoadSocialCommandAsync";
                Logger.Report(ex);
            }
            finally
            {
                LoadingSocial = false;
            }

        }

        bool socialError;
        public bool SocialError
        {
            get { return socialError; }
            set {
                socialError = value;
                RaisePropertyChanged();
            }
        }

        Tweet selectedTweet;
        public Tweet SelectedTweet
        {
            get { return selectedTweet; }
            set
            {
                selectedTweet = value;
                RaisePropertyChanged();
                if (selectedTweet == null)
                    return;

                LaunchBrowserCommand.Execute(selectedTweet.Url);

                SelectedTweet = null;
            }
        }

        #endregion
    }
}

