using System.Threading.Tasks;

using Xamarin.Forms;

using Plugin.Settings;
using Plugin.Settings.Abstractions;

using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Mock
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        #region AppSettings Singleton

        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #endregion

        #region Favorites

        public static bool IsFavorite(string id) =>
            AppSettings.GetValueOrDefault<bool>("fav_" + id, false);

        public static void SetFavorite(string id, bool favorite) =>
            AppSettings.AddOrUpdateValue("fav_" + id, favorite);

        public static async Task ClearFavorites()
        {
            var lockers = await DependencyService.Get<ILockerMemberStore>().GetItemsAsync();
            foreach (var locker in lockers)
                AppSettings.Remove("fav_" + locker.Id);
        }

        #endregion

        #region Feedback

        public static bool LeftFeedback(string id) =>
        AppSettings.GetValueOrDefault<bool>("feed_" + id, false);

        public static void LeaveFeedback(string id, bool leave) =>
        AppSettings.AddOrUpdateValue("feed_" + id, leave);

        public static async Task ClearFeedback()
        {
            var lockers = await DependencyService.Get<ILockerMemberStore>().GetItemsAsync();
            foreach (var locker in lockers)
                AppSettings.Remove("feed_" + locker.Id);
        }

        #endregion
    }
}