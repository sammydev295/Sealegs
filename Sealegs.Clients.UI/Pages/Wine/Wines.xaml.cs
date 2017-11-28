using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI.Pages.Wine
{
    public partial class Wines : ContentPage
    {
        private readonly WinesViewModel _viewModel = App.Locator.WinesViewModel;

        public Wines()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.Initialize();
        }

        
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem==null)return;

            var wine = e.SelectedItem as DataObjects.Wine;
            _viewModel.ItemSelected(wine);
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