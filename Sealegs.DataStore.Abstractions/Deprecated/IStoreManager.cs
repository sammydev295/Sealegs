using System;
using System.Threading.Tasks;

namespace Sealegs.DataStore.Abstractions
{
    public interface IStoreManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();

      //  ILockerMemberStore LockerMemberStore { get; }

        #region Not Used

        ICategoryStore CategoryStore { get; }
        IFavoriteStore FavoriteStore { get; }
        IFeedbackStore FeedbackStore { get; }
        ILockerMemberStore LockerStore { get; }
        INewsStore NewsStore { get; }
        IEventStore EventStore { get; }
        IWineStore WineStore { get; }
        INotificationStore NotificationStore { get; }
        IUserStore UserStore { get; }
        IMiniHacksStore MiniHacksStore { get; }

        #endregion

        Task<bool> SyncAllAsync(bool syncUserSpecific);
        Task DropEverythingAsync();
      
    }
}

