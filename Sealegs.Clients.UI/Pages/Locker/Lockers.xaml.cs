using System;
using Xamarin.Forms;
using Sealegs.DataObjects;
using FormsToolkit;
using Sealegs.Clients.Portable;
using Sealegs.Clients.UI.Pages.Employees;
using Sealegs.Clients.UI.Pages.Sessions;
//using Sealegs.Clients.UI.Pages.Sponsors;

namespace Sealegs.Clients.UI
{
    public partial class Lockers : ContentPage
    {
        LockersViewModel ViewModel => vm ?? (vm = BindingContext as LockersViewModel);
        LockersViewModel vm;
        string loggedIn;
        public Lockers()
        {
            InitializeComponent();
            loggedIn = Settings.Current.Email;
         

            BindingContext = vm = new LockersViewModel(Navigation);

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "toolbar_refresh.png",
                    Command = vm.ForceRefreshCommand
                });
            }

            ListViewLockers.ItemSelected +=  (sender, e) => 
                {
                    var session = ListViewLockers.SelectedItem as LockerMember;
                    if(session == null)
                        return;
                };
        }

        void ListViewTapped (object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }
       
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListViewLockers.ItemTapped += ListViewTapped;

            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Subscribe("filter_changed", (d) => UpdatePage());
            
            UpdatePage();

        }

        void UpdatePage()
        {
            Title =  "Lockers";

            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow)) ||
                loggedIn != Settings.Current.Email;

            loggedIn = Settings.Current.Email;
            //Load if none, or if 45 minutes has gone by
            if ((ViewModel?.Lockers?.Count ?? 0) == 0 || forceRefresh)
            {
                ViewModel?.LoadSessionsCommand?.Execute(forceRefresh);
            }
        }

        protected override void OnDisappearing()

        {
            base.OnDisappearing();
            ListViewLockers.ItemTapped -= ListViewTapped;
            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Unsubscribe("filter_changed");
        }

        public void OnResume()
        {
            UpdatePage();
        }
    }
}

