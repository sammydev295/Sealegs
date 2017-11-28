using System;
using System.Threading.Tasks;
using System.Linq;

using FormsToolkit;
using Xamarin.Forms;

using DLToolkit.Forms.Controls;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.UI.Pages.Wine;

namespace Sealegs.Clients.UI
{
    public partial class LockersPage : ContentPage
    {
        #region LockersViewModel

        private readonly LockersViewModel _viewModel = App.Locator.LockersViewModel;
        
        #endregion

        #region CTOR

        public LockersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.Initialize();

            MessagingService.Current.Subscribe<Sealegs.DataObjects.LockerMember>(MessageKeys.UpdateLocker, (m, locker) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var theLocker = _viewModel.LockersFiltered.First(l => l.Id == locker?.Id);
                    theLocker = locker;
                    theLocker.ProfileImage = LockersViewModel.LockerMemberDb.GetFullImageName(locker.ProfileImage);
                });
            });
            MessagingService.Current.Subscribe<Sealegs.DataObjects.LockerMember>(MessageKeys.InsertLocker, (m, locker) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _viewModel.Lockers.Add(locker);
                    _viewModel.LockersFiltered.Add(locker);
                });
            });
            MessagingService.Current.Subscribe<string>(MessageKeys.UpdateLocker, (m, image) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var selected = _viewModel.SelectedLocker;
                    selected.ProfileImage = LockersViewModel.LockerMemberDb.GetFullImageName(image);
                });
            });
        }

        #endregion

        #region ListView_OnItemSelected

        private void OnLockerSelected(object sender, ItemTappedEventArgs e)
        {
            var locker = e.Item as LockerMember;
            _viewModel.SelectedLocker = locker;
        }

        #endregion
    }
}

