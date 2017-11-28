using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI.Pages.Wine
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddWinePage : PopupPage
    {
        #region Properties


        #endregion

        #region ViewModel

        private readonly AddWineViewModel _viewModel = App.Locator.AddWineViewModel;

        #endregion

        #region CTOR (2 Overloads)

        public AddWinePage(LockerMember locker, List<WineVarietal> wineVarietals)
        {
            InitializeComponent();
            BindingContext = _viewModel;

            _viewModel.Locker = locker;
            _viewModel.ButtonName = "Check In";
            _viewModel.PageTitle = "Add Wine";
            _viewModel.Initialize(locker, wineVarietals);
        }

        public AddWinePage(DataObjects.Wine wine, List<WineVarietal> wineVarietals)
        {
            InitializeComponent();
            BindingContext = _viewModel;

            _viewModel.ButtonName = "Update";
            _viewModel.PageTitle = wine.WineTitle;
            _viewModel.Initialize(wine, wineVarietals);
        }
      
        #endregion

        #region Cancel_OnClicked

        private void Cancel_OnClicked(object sender, EventArgs e)
        {
            (_viewModel._navService as ISealegsNavigationService).PopupGoBack();
        }

        #endregion
    }
}
