using System;
using Sealegs.DataStore.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sealegs.DataStore.Mock
{
    public class StoreManager : IStoreManager
    {
        #region IStoreManager implementation

        public Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            return Task.FromResult(true);
        }

        public bool IsInitialized { get { return true; }  }
        public Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        #endregion

        public Task DropEverythingAsync()
        {
            return Task.FromResult(true);
        }

        public void SetInitlize(bool value)
        {
           
        }

        INotificationStore notificationStore;
        public INotificationStore NotificationStore => notificationStore ?? (notificationStore  = DependencyService.Get<INotificationStore>());

        IMiniHacksStore miniHacksStore;
        public IMiniHacksStore MiniHacksStore => miniHacksStore ?? (miniHacksStore  = DependencyService.Get<IMiniHacksStore>());

        ICategoryStore categoryStore;
        public ICategoryStore CategoryStore => categoryStore ?? (categoryStore  = DependencyService.Get<ICategoryStore>());

        IFavoriteStore favoriteStore;
        public IFavoriteStore FavoriteStore => favoriteStore ?? (favoriteStore  = DependencyService.Get<IFavoriteStore>());

        IFeedbackStore feedbackStore;
        public IFeedbackStore FeedbackStore => feedbackStore ?? (feedbackStore  = DependencyService.Get<IFeedbackStore>());

        ILockerMemberStore lockerStore;
        public ILockerMemberStore LockerStore => lockerStore ?? (lockerStore  = DependencyService.Get<ILockerMemberStore>());

        IWineStore wineStore;
        public IWineStore WineStore => wineStore ?? (wineStore = DependencyService.Get<IWineStore>());

        IWineVarietalStore wineVarietalStore;
        public IWineVarietalStore WineVarietalStore => wineVarietalStore ?? (wineVarietalStore = DependencyService.Get<IWineVarietalStore>());

        IEventStore eventStore;
        public IEventStore EventStore => eventStore ?? (eventStore = DependencyService.Get<IEventStore>());

        INewsStore newsStore;
        public INewsStore NewsStore => newsStore ?? (newsStore  = DependencyService.Get<INewsStore>());

        IUserStore userStore;
        public IUserStore UserStore => userStore ?? (userStore = DependencyService.Get<IUserStore>());
    }
}

