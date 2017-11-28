using System;
using System.Threading.Tasks;
using System.Linq;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Sealegs.DataStore.Abstractions.REST;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.Portable
{
    public class FavoriteService : IFavoriteService
    {
        #region CTOR

        public FavoriteService()
        {
        }

        #endregion 

        #region ToggleFavorite

        public async Task<bool> ToggleFavorite(LockerMember locker)
        {
            try
            {
                var store = DependencyService.Get<IFavorite>();
                if (!locker.IsFavorite.HasValue)
                    locker.IsFavorite = false;
                locker.IsFavorite = !locker.IsFavorite;//switch first so UI updates :)
                if (!locker.IsFavorite.Value)
                {
                    DependencyService.Get<ILogger>().Track(SealegsLoggerKeys.FavoriteRemoved, "Title", locker.DisplayName);
                    var items = await store.GetAll(locker.Id);
                    foreach (var item in items.Where(s => s.FavoriteTypeId == locker.Id && s.UserId == locker.UserID))
                    {
                        await store.Delete(item.Id);
                    }
                }
                else
                {
                    DependencyService.Get<ILogger>().Track(SealegsLoggerKeys.FavoriteAdded, "Title", locker.DisplayName);
                    await store.Insert(new Favorite { FavoriteTypeId = locker.Id, UserId = locker.UserID, FavoriteType = "locker" });
                }

                //Settings.Current.LastFavoriteTime = DateTime.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "ToggleFavorite");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }

            return false;
        }

        #endregion 
    }
}

