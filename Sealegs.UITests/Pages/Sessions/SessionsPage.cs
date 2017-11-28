using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Linq;
using Xamarin.UITest.Android;

namespace Sealegs.UITests
{
    public class LockersPage : BasePage
    {
        readonly Query SearchField;
        readonly Query LockerCellContainer;
        readonly Query FilterButton;
        readonly Query FavoriteButton;

        readonly string LoadingMessage = "Loading Lockers...";

        public LockersPage()
            : base ("Lockers", "Lockers")
        {
            if (OnAndroid)
            {
                SearchField = x => x.Id("search_src_text");
                LockerCellContainer = x => x.Class("ViewCellRenderer_ViewCellContainer").Index(1);
                FilterButton = x => x.Marked("Filter");
                FavoriteButton = x => x.Marked("FavoriteButton");
            }
            if (OniOS)
            {
                SearchField = x => x.Class("UISearchBarTextField");
                LockerCellContainer = x => x.Class("Xamarin_Forms_Platform_iOS_ViewCellRenderer_ViewTableCell");
                FilterButton = x => x.Class("UINavigationButton").Marked("Filter");
                FavoriteButton = x => x.Marked("FavoriteButton");
            }
        }

        public LockersPage EnterSearchAndVerify(string search, bool valid)
        {
            app.Tap(SearchField);
            app.EnterText(search);
            app.Screenshot("Entered text: " + search);
            app.DismissKeyboard();

            app.WaitForNoElement(LoadingMessage);

            bool result = app.Query(LockerCellContainer).Any();
            Assert.IsTrue(result == valid, String.Format("Expected search to be {0}, but was {1}", valid, result));
            app.Screenshot("Locker search: " + search);

            if (!valid)
                Assert.IsNotEmpty(app.Query("No Lockers Found"));

            return this;
        }

        public void FavoriteFirstLocker()
        {
            app.Tap(FavoriteButton);
            app.Screenshot("Tapped Favorite Button");
        }

        public void GoToFilterLockersPage()
        {
            app.Screenshot("Tapping Filter Button");
            app.Tap(FilterButton);
        }

        public void InvestigateFirstLocker()
        {
            app.Screenshot("Investigating First Locker");
            app.Tap(LockerCellContainer);
        }

        public void InvestigateLockerMarked(string title)
        {
            app.Screenshot("Selecting locker: " + title);
            app.Tap(title);
        }

        public void ValidateFavorite()
        {
            app.Screenshot("Favorite Button is filled in");
        }
    }
}

