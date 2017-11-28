using System;
using System.Collections.Generic;
using System.Linq;

using AsNum.XFControls;
using FormsToolkit;
using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI.Pages.Wine
{
    public partial class WinesCarouselPage : ContentPage
    {
        #region ViewModel

        private readonly WineCarouselViewModel _viewModel = App.Locator.WineCarouselViewModel;

        #endregion 

        #region CTOR

        public WinesCarouselPage(Tuple<LockerMember, List<WineVarietal>> parameters)
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.Locker = parameters.Item1;
            _viewModel.WineVarietals = parameters.Item2;
            _viewModel.LoadWinesCommand.Execute(null);

            MessagingService.Current.Subscribe<Sealegs.DataObjects.Wine>(MessageKeys.UpdateWine, (m, wine) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var pos = WineCarousel.Position;
                    _viewModel.LoadWinesCommand.Execute(null);
                    WineCarousel.Position = pos;
                });
            });
            MessagingService.Current.Subscribe<Sealegs.DataObjects.Wine>(MessageKeys.InsertWine, (m, wine) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var pos = WineCarousel.Position;
                    _viewModel.LoadWinesCommand.Execute(null);
                    WineCarousel.Position = pos;
                });
            });
        }

        #endregion

        #region Event Handler method Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadWinesCommand.Execute(null);
            //  ListViewLockers.ItemTapped += ListViewTapped;
            //UpdatePage();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //  ListViewLockers.ItemTapped -= ListViewTapped;

        }

        public void OnResume()
        {
            //UpdatePage();
        }

        #endregion

        #region CarouselView_OnItemSelected

        private void CarouselView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as DataObjects.Wine;
            if (item == null) return;

            _viewModel.ItemSelected(item);
        }

        #endregion

        #region EditWine

        private void EditWine(object sender, EventArgs e)
        {
            _viewModel.EditWine();
        }

        #endregion

        #region DeleteWine

        private void DeleteWine(object sender, EventArgs e)
        {
            _viewModel.DeleteWine();
        }

        #endregion
    }
}
