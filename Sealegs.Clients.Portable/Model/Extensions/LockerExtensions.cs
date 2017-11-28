using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using MvvmHelpers;
using NodaTime;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using BaseViewModel = Sealegs.Clients.Portable.ViewModel.BaseViewModel;

namespace Sealegs.Clients.Portable
{
    public static class LockerExtensions
    {
        public static async Task<Stream> GetStreamFromImageSourceAsync(this StreamImageSource imageSource, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (imageSource.Stream != null)
            {
                return await imageSource.Stream(cancellationToken);
            }

            return null;
        }


        public static AppLinkEntry GetAppLink(this LockerMember locker)
        {
            var url = $"http://evolve.xamarin.com/locker/{locker.Id.ToString()}";
            
            var entry = new AppLinkEntry
            {
                Title = locker.DisplayName,
                Description = locker.MemberName,
                AppLinkUri = new Uri(url, UriKind.RelativeOrAbsolute),
                IsLinkActive = true
            };

            if (Device.OS == TargetPlatform.iOS)
                entry.Thumbnail = ImageSource.FromFile("Icon.png");

            entry.KeyValues.Add("contentType", "Locker");
            entry.KeyValues.Add("appName", "Evolve16");
            entry.KeyValues.Add("companyName", "Xamarin");

            return entry;
        }

        public static string GetIndexName(this LockerMember e)
        {
            if (!e.CurrentRenewalDate.HasValue || !e.NextRenewalDate.HasValue || e.CurrentRenewalDate.Value.IsTBA())
                return "To be announced";

            var start = e.CurrentRenewalDate.Value.ToEasternTimeZone();

            var startString = start.ToString("t"); 
            var end = e.NextRenewalDate.Value.ToEasternTimeZone();
            var endString = end.ToString("t");

            var day = start.DayOfWeek.ToString();
            var monthDay = start.ToString("M");
            return $"{day}, {monthDay}, {startString}–{endString}";
        }

        public static string GetSortName(this LockerMember locker)
        {
            if (!locker.CurrentRenewalDate.HasValue || !locker.NextRenewalDate.HasValue  || locker.CurrentRenewalDate.Value.IsTBA())
                return "To be announced";
            
            var start = locker.CurrentRenewalDate.Value.ToEasternTimeZone();
            var startString = start.ToString("t"); 

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today {startString}";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow {startString}";
            }
            var day = start.ToString("M");
            return $"{day}, {startString}";
        }

        public static string GetDisplayName(this LockerMember locker)
        {
            if (!locker.CurrentRenewalDate.HasValue || !locker.NextRenewalDate.HasValue || locker.CurrentRenewalDate.Value.IsTBA())
                return "To be announced";

            var start = locker.CurrentRenewalDate.Value.ToEasternTimeZone();
            var startString = start.ToString("t"); 
            var end = locker.NextRenewalDate.Value.ToEasternTimeZone();
            var endString = end.ToString("t");

           

            if (DateTime.Today.Year == start.Year)
            {
                if (DateTime.Today.DayOfYear == start.DayOfYear)
                    return $"Today {startString}–{endString}";

                if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
                    return $"Tomorrow {startString}–{endString}";
            }
            var day = start.ToString("M");
            return $"{day}, {startString}–{endString}";
        }

        public static string GetDisplayTime(this LockerMember locker)
        {
            if (!locker.CurrentRenewalDate.HasValue || !locker.NextRenewalDate.HasValue || locker.CurrentRenewalDate.Value.IsTBA())
                return "To be announced";

            var start = locker.CurrentRenewalDate.Value.ToEasternTimeZone();

            var startString = start.ToString("t"); 
            var end = locker.NextRenewalDate.Value.ToEasternTimeZone();
            var endString = end.ToString("t");
            return $"{startString}–{endString}";
        }

        public static IOrderedEnumerable<LockerMember> FilterLastUsed(this IList<LockerMember> lockers)
        {
            //if (Settings.Current.FavoritesOnly)
            //    lockers = lockers.Where(s => s.IsFavorite).ToList();

            var ordered = lockers.Where(s => s.IsActive.Value ).OrderBy(l => l.LastCheckedOutDate!=null?l.LastCheckedOutDate.Value:DateTime.Today);
            var utc = DateTime.UtcNow;

            return ordered;
        }

        public async static Task<IEnumerable<LockerMember>> FilterFavorites(this IList<LockerMember> lockers)
        {
            //if (Settings.Current.FavoritesOnly)
            //    lockers = lockers.Where(s => s.IsFavorite).ToList();
            try
            {
                var favoriteStore = DependencyService.Get<IFavoriteStore>();
                IList<Favorite> favoriteLockers = await favoriteStore.GetFavorites(BaseViewModel.User.Id, "locker");
                var favLockers = from f in favoriteLockers
                    join l in lockers on f.UserId equals l.UserID
                    select l;

                return favLockers;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static IEnumerable<LockerMember> FilterInActive(this IList<LockerMember> lockers)
        {
            //if (Settings.Current.FavoritesOnly)
            //    lockers = lockers.Where(s => s.IsFavorite).ToList();
            var store = DependencyService.Get<ILockerMemberStore>();
            var ordered = lockers.Where(s => !s.IsActive.Value).OrderBy(l => l.LastCheckedOutDate != null ? l.LastCheckedOutDate.Value : DateTime.Today);
            var utc = DateTime.UtcNow;

            return ordered;
        }

        public static IEnumerable<LockerMember> Search(this IEnumerable<LockerMember> lockers, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return lockers;

            var searchSplit = searchText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //search display name
            return lockers.Where(locker => 
                                  searchSplit.Any(search => 
                                locker.DisplayName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
}

