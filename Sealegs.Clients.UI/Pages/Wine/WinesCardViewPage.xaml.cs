using System;
using System.Linq;

using AsNum.XFControls;
using FormsToolkit;
using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI.Pages.Wine
{
    public partial class WinesCardViewPage : ContentPage
    {
        #region ViewModel

        private readonly WinesCardVieModel _viewModel = App.Locator.WinesCardViewPageViewModel;

        #endregion

        #region CTOR (2 overloads)

        public WinesCardViewPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = _viewModel;
                _viewModel.Initialize();
            }
            catch (Exception e)
            {

            }
        }

        public WinesCardViewPage(LockerMember locker)
        {
            try
            {
                InitializeComponent();
                BindingContext = _viewModel;
                _viewModel.Initialize(locker);
            }
            catch (Exception e)
            {

            }           
        }

        #endregion

        #region Swipes

        void SwipeLeft(object sender, int index)
        {
            // card swiped to the left
            _viewModel.SelectedCardItem = cardStack.ItemsSource[index];
        }

        void SwipeRight(object sender, int index)
        {
            // card swiped to the right
            _viewModel.SelectedCardItem = cardStack.ItemsSource[index];
        }

        #endregion

        #region Event Overrides 

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

        }

        public void OnResume()
        {
        }

        #endregion
    }
}
