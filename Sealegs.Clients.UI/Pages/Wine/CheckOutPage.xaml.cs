using System;

using FormsToolkit;
using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Clients.UI.Pages.Wine;

namespace Sealegs.Clients.UI
{
    public partial class CheckOutPage : ContentPage
    {
        #region Properties 

        private readonly CheckOutViewModel _viewModel = App.Locator.CheckOutViewModel;

        //string loggedIn;


        #endregion

        #region CTOR

        public CheckOutPage(LockerMember locker)
        {
          
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.LockerMember = locker;
            _viewModel.Intialize();
        }

        #endregion

        #region Event Handler Method Overrides

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    //  ListViewLockers.ItemTapped += ListViewTapped;
        //    UpdatePage();
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    //  ListViewLockers.ItemTapped -= ListViewTapped;

        //}

        //public void OnResume()
        //{
        //    UpdatePage();
        //}

        #endregion

        #region UpdatePage

        //void UpdatePage()
        //{
        //    bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow)) || loggedIn != Settings.Current.Email;

        //    loggedIn = Settings.Current.Email;

        //    // Load if none, or if 45 minutes has gone by
        //    if ((ViewModel?.Wines?.Count ?? 0) == 0 || forceRefresh)
        //        ViewModel?.LoadWinesCommand?.Execute(forceRefresh);
        //}

        #endregion

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Wine;
            if(item == null) return;

            item.IsChecked = !item.IsChecked;
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var list = sender as ListView;
            if (list == null)
                return;

            list.SelectedItem = null;
        }

        private void FlowListView_OnFlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Wine;
            if (item == null) return;

            item.IsChecked = !item.IsChecked;
        }
    }
}
