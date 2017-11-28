using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MvvmHelpers;
using Rg.Plugins.Popup.Pages;

using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEditLockerPage : PopupPage
    {
        #region ViewModel

        private readonly AddEditLockersViewModel _viewModel = App.Locator.AddEditLockersViewModel;

        #endregion

        #region CTOR (2 overloads)

        public AddEditLockerPage(LockerMember locker, List<LockerType> lockerTypes)
        {
            try
            {
                InitializeComponent();

                Task.Delay(500);
                BindingContext = _viewModel;
                _viewModel.Locker = locker;
                _viewModel.AllLockerTypes = new ObservableRangeCollection<LockerType>(lockerTypes);

                if (locker == null)
                    _viewModel.SelectedLockerType = lockerTypes.First();
                else
                    _viewModel.SelectedLockerType = lockerTypes.Any(lt => lt.LockerTypeID.ToString().ToUpper() == _viewModel.Locker.LockerTypeID.ToUpper()) ? lockerTypes.First(lt => lt.LockerTypeID.ToString().ToUpper() == _viewModel.Locker.LockerTypeID.ToUpper()) : lockerTypes.First();
                _viewModel.PageName = locker.DisplayName;
                _viewModel.IsPasswordEnabled = false;
                _viewModel.IsEmailEnabled = false;
                _viewModel.ButtonName = "Update";
            }
            catch (Exception e)
            {
            }
        }

        public AddEditLockerPage(List<LockerType> lockerTypes)
        {
            InitializeComponent();

            Task.Delay(500);
            BindingContext = _viewModel;
            _viewModel.Locker = LockerMember.Defaults;
            _viewModel.AllLockerTypes = new ObservableRangeCollection<LockerType>(lockerTypes);
            _viewModel.SelectedLockerType = lockerTypes.First();
            _viewModel.PageName = "Add Locker";
            _viewModel.IsPasswordEnabled = true;
            _viewModel.IsEmailEnabled = true;
            _viewModel.ButtonName = "Save";
        }

        #endregion

        #region Event Overrides

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        #endregion 

        #region Cancel_OnClicked

        private async void Cancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        #endregion
    }
}
