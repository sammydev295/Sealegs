using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.Clients.UI
{
    public partial class NotificationsManagmentPage : ContentPage
    {
        private readonly NotificationsManagmentViewModel _viewModel = App.Locator.NotificationsManagmentViewModel;
        public NotificationsManagmentPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;

            _viewModel.Initialize();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Notification;
            if(item==null) return;

            _viewModel.HandleSelection(item);
        }
    }

}
