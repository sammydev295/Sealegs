using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI.Pages.ChillRequest
{
    public partial class RemoteChillRequestPage : ContentPage
    {
        private readonly RemoteChillRequestViewModel _viewModel = App.Locator.RemoteChillRequestViewModel;

        public RemoteChillRequestPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Intialize();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as RemoteChillRequest;
            if(item==null)return;

            _viewModel.ItemSelected();
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;
            list.SelectedItem = null;
        }

       
    }
}