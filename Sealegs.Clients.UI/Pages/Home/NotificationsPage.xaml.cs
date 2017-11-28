using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public partial class NotificationsPage : ContentPage
    {
        private NotificationsViewModel ViewModel = App.Locator.NotificationsViewModel;
        public NotificationsPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
            ListViewNotifications.ItemTapped += (sender, e) => ListViewNotifications.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel.Notifications.Count == 0)
                ViewModel.LoadNotificationsCommand.Execute(false);
        }
    }
}

