using System;

namespace Sealegs.UITests
{
    public class FilterLockersPage : BasePage
    {
        readonly string DoneButton = "Done";

        public FilterLockersPage()
            : base ("Filter Lockers", "Filter Lockers")
        {
            if (OnAndroid)
            {
            }
            if (OniOS)
            {
            }
        }

        public FilterLockersPage SelectLockerFilters(
            bool PastLockers = false, 
            bool FavoritesOnly = false)
        {
            if (OniOS)
            {
                app.Query(x => x.Class("UISwitch").Marked("Show Past Lockers").Invoke("setOn", PastLockers, "animated", true));
                app.Screenshot("'Show Past Lockers' set to: " + PastLockers);

                app.Query(x => x.Class("UISwitch").Marked("Show Favorites Only").Invoke("setOn", FavoritesOnly, "animated", true));
                app.Screenshot("'Show Favorites Only' set to: " + FavoritesOnly);
            }
            if (OnAndroid)
            {
                app.Query(x => x.Marked("Show Past Lockers").Parent(1).Descendant().Class("android.widget.Switch").Invoke("setChecked", PastLockers));
                app.Screenshot("'Show Past Lockers' set to: " + PastLockers);

                app.Query(x => x.Marked("Show Favorites Only").Parent(1).Descendant().Class("android.widget.Switch").Invoke("setChecked", FavoritesOnly));
                app.Screenshot("'Show Favorites Only' set to: " + FavoritesOnly);
            }

            return this;
        }

        public FilterLockersPage SelectLockerCategories(
            bool DisplayAll = true, 
            bool Android = true, 
            bool iOS = true,
            bool XamarinForms = true,
            bool Design = true,
            bool Secure = true,
            bool Test = true,
            bool Monitor = true)
        {
            if (DisplayAll)
            {
                app.Screenshot("'Display All Selected, no other category filters enabled'");
                return this;
            }

            if (OniOS)
            {
                app.Query(x => x.Class("UISwitch").Marked("Display All").Invoke("setOn", DisplayAll, "animated", true));
                app.Screenshot("'Display All' set to: " + DisplayAll);

                app.Query(x => x.Class("UISwitch").Marked("Android").Invoke("setOn", Android, "animated", true));
                app.Screenshot("'Android' set to: " + Android);

                app.Query(x => x.Class("UISwitch").Marked("iOS").Invoke("setOn", iOS, "animated", true));
                app.Screenshot("'iOS' set to: " + iOS);

                app.Query(x => x.Class("UISwitch").Marked("Xamarin.Forms").Invoke("setOn", XamarinForms, "animated", true));
                app.Screenshot("'Xamarin.Forms' set to: " + XamarinForms);

                app.Query(x => x.Class("UISwitch").Marked("Design").Invoke("setOn", Design, "animated", true));
                app.Screenshot("'Design' set to: " + Design);

                app.Query(x => x.Class("UISwitch").Marked("Secure").Invoke("setOn", Secure, "animated", true));
                app.Screenshot("'Secure' set to: " + Secure);

                app.Query(x => x.Class("UISwitch").Marked("Test").Invoke("setOn", Test, "animated", true));
                app.Screenshot("'Test' set to: " + Test);

                app.Query(x => x.Class("UISwitch").Marked("Monitor").Invoke("setOn", Monitor, "animated", true));
                app.Screenshot("'Monitor' set to: " + Monitor);
            }
            if (OnAndroid)
            {
                app.Query(x => x.Marked("Display All").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", DisplayAll));
                app.Screenshot("'Display All' set to: " + DisplayAll);

                app.Query(x => x.Marked("Android").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", Android));
                app.Screenshot("'Android' set to: " + Android);

                app.Query(x => x.Marked("iOS").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", iOS));
                app.Screenshot("'iOS' set to: " + iOS);

                app.Query(x => x.Marked("Xamarin.Forms").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", XamarinForms));
                app.Screenshot("'Xamarin.Forms' set to: " + XamarinForms);

                app.Query(x => x.Marked("Design").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", Design));
                app.Screenshot("'Design' set to: " + Design);

                app.Query(x => x.Marked("Secure").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", Secure));
                app.Screenshot("'Secure' set to: " + Secure);

                app.Query(x => x.Marked("Test").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", Test));
                app.Screenshot("'Test' set to: " + Test);

                app.Query(x => x.Marked("Monitor").Parent(1).Descendant().Class("SwitchCompat").Invoke("setChecked", Monitor));
                app.Screenshot("'Monitor' set to: " + Monitor);
            }

            return this;
        }

        public void CloseLockerFilter()
        {
            app.Tap(DoneButton);
        }
       
    }
}

