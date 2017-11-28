using System;
using System.Collections.ObjectModel;
using Sealegs.Clients.UI.Pages.ChillRequest;
using Sealegs.Clients.UI.Pages.Wine;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Clients.UI.Pages.Evaluation;
using Sealegs.Clients.UI.Pages.Support;
using Sealegs.Utils;

namespace Sealegs.Clients.UI.Pages.Android.Menu
{
    public partial class Master : MasterDetailPage
    {
        #region Menus

        public ObservableCollection<MasterMenuItem> PreLoginItems()
        {
            var list = new ObservableCollection<MasterMenuItem>()
            {
                new MasterMenuItem {Id = 0, Title = "News", TargetType = typeof(NewsPage), Icon = "menu_feed.png"},
                new MasterMenuItem {Id = 1, Title = "Manage Events", TargetType = typeof(EventsPage), Icon = "menu_events.png"},
                new MasterMenuItem {Id = 2, Title = "Resturant Maps", TargetType = typeof(FloorMapsCarouselPage), Icon = "menu_plan.png"},
                new MasterMenuItem {Id = 4, Title = "Contact Us", TargetType = typeof(ContactUs), Icon = "menu_news.png"},
                new MasterMenuItem {Id = 5, Title = "About Us", TargetType = typeof(AboutUs), Icon = "aboutUs_icon.png"}
            };
            if (Device.OS == TargetPlatform.iOS)
                list.Add(new MasterMenuItem { Id = 6, Title = "Push Notifications", TargetType = typeof(AboutPage), Icon = "tick.png" });

            return list;
        }

        public ObservableCollection<MasterMenuItem> PostLoginItems(User user)
        {
            ObservableCollection<MasterMenuItem> list = null;
            if (user.Role == SealegsUserRole.AdminRole || user.Role == SealegsUserRole.ManagerRole)
            {
                list = new ObservableCollection<MasterMenuItem>()
                {
                    new MasterMenuItem { Id = 0, Title = "Manage Lockers", TargetType = typeof(LockersPage), Icon = "lockers_icon.png" },
                    new MasterMenuItem { Id = 1, Title = "Manage Employees", TargetType = typeof(Employees), Icon = "Employee_Icon.png" },
                    new MasterMenuItem { Id = 2, Title = "Manage Events", TargetType = typeof(EventsPage), Icon = "menu_events.png" },
                    new MasterMenuItem { Id = 3, Title = "Manage News", TargetType = typeof(NewsPage), Icon = "menu_feed.png" },
                    new MasterMenuItem { Id = 4, Title = "Manage Notifications", TargetType = typeof(NotificationsManagmentPage), Icon = "notification_icon.png" },
                    new MasterMenuItem { Id = 5, Title = "View Chill Requests", TargetType = typeof(RemoteChillRequestPage), Icon = "chill_icon.png" },
                    new MasterMenuItem { Id = 6, Title = "View Wine/Event Rating", TargetType = typeof(WineEvaluation), Icon = "menu_events.png" },
                    new MasterMenuItem { Id = 7, Title = "Resturant Maps", TargetType = typeof(FloorMapsCarouselPage), Icon = "menu_plan.png" },
                    new MasterMenuItem { Id = 8, Title = "Contact Us", TargetType = typeof(ContactUs), Icon = "contact_icon.png" },
                    new MasterMenuItem { Id = 9, Title = "About Us", TargetType = typeof(AboutUs), Icon = "aboutUs_icon.png" },
                    new MasterMenuItem { Id = 10, Title = "Logout", TargetType = typeof(LoginPage), Icon = "logout_icon.png" }
                };
            }
            else if (user.Role == SealegsUserRole.EmployeeRole)
            {
                list = new ObservableCollection<MasterMenuItem>()
                {
                    new MasterMenuItem { Id = 0, Title = "View Chill Requests", TargetType = typeof(RemoteChillRequestPage), Icon = "chill_icon.png" },
                    new MasterMenuItem { Id = 1, Title = "View Wine/Event Rating", TargetType = typeof(WineEvaluation), Icon = "menu_events.png" },
                    new MasterMenuItem { Id = 2, Title = "Resturant Maps", TargetType = typeof(FloorMapsCarouselPage), Icon = "menu_plan.png" },
                    new MasterMenuItem { Id = 3, Title = "Contact Us", TargetType = typeof(ContactUs), Icon = "contact_icon.png" },
                    new MasterMenuItem { Id = 4, Title = "About Us", TargetType = typeof(AboutUs), Icon = "aboutUs_icon.png" },
                    new MasterMenuItem { Id = 5, Title = "Logout", TargetType = typeof(LoginPage), Icon = "logout_icon.png" },
                };
            }
            else if (user.Role == SealegsUserRole.LockerMemberRole || user.Role == SealegsUserRole.LockerMemberFriendRole || user.Role == SealegsUserRole.CustomerRole)
            {
                list = new ObservableCollection<MasterMenuItem>()
                {
                    new MasterMenuItem { Id = 0, Title = "My Wines", TargetType = typeof(Wines), Icon = "menu_locker.png" },
                    new MasterMenuItem { Id = 1, Title = "Rate/Chill/Share Wines", TargetType = typeof(WinesCardViewPage), Icon = "btnWineList.png" },
                    new MasterMenuItem { Id = 2, Title = "Resturant Maps", TargetType = typeof(FloorMapsCarouselPage), Icon = "menu_plan.png" },
                    new MasterMenuItem { Id = 3, Title = "Contact Us", TargetType = typeof(ContactUs), Icon = "contact_icon.png" },
                    new MasterMenuItem { Id = 4, Title = "About Us", TargetType = typeof(AboutUs), Icon = "aboutUs_icon.png" },
                    new MasterMenuItem { Id = 5, Title = "Logout", TargetType = typeof(LoginPage), Icon = "logout_icon.png" },
                };
            }

            if (Device.OS == TargetPlatform.iOS)
                list.Add(new MasterMenuItem { Id = 11, Title = "Enable Push Notifications", TargetType = typeof(AboutPage), Icon = "tick.png" });

            return list;
        }

        #endregion

        #region Fields

        private readonly MasterViewModel _viewModel = App.Locator.MasterViewModel;
        bool _isRunning;

        #endregion

        #region CTOR (2 Overloads)

        public Master()
        {
            Intialize();

            //Setting Master Page Properties
            _viewModel.Username = "Sign In";
            _viewModel.ProfilePath = String.Empty;
        }

        public Master(User user)
        {
            Intialize(user);
            _viewModel.User = user;
            _viewModel.ProfilePath = ImageSource.FromUri(new Uri(Gravatar.GetURL(user.Email)));
        }

        
        #endregion
     
        #region Intialize (2 overloads)

        private  void Intialize(User user)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            MenuItemsListView.ItemSelected += ListView_ItemSelected;

            // Setting Menu List
            var list = PostLoginItems(user);
            Settings.Current.FirstRun = true;
            MenuItemsListView.ItemsSource = list;
            Detail = new SealegsNavigationPage((Page)Activator.CreateInstance(list[0].TargetType));

        }

        private void Intialize()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            NavigationPage.SetHasNavigationBar(this, false);
            MenuItemsListView.ItemSelected += ListView_ItemSelected;

            //Setting Menu List
            var list = PreLoginItems();
            Settings.Current.FirstRun = true;
            MenuItemsListView.ItemsSource = list;
            Detail = new SealegsNavigationPage((Page)Activator.CreateInstance(list[0].TargetType));
        }

        #endregion

        #region Event Handlers & Overrides

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterMenuItem;
            if (item == null)
                return;

            if (item.Icon == "logout_icon.png")
                _viewModel.Logout();

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;
            Detail = new NavigationPage(page);
            IsPresented = false;

            MenuItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel.User != null && _viewModel.User.Role == SealegsUserRole.LockerMemberRole &&
                _viewModel.LockerMemberUser != null)
            {
                _viewModel.ProfilePath = String.Concat(Addresses.LockersStorageBaseAddress, _viewModel.LockerMemberUser.ProfileImage);
                _viewModel.Username = _viewModel.LockerMemberUser.DisplayName;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        #endregion
    }
}