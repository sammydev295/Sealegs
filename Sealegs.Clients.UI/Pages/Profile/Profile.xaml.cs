using System;
using Sealegs.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI.Pages.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        private readonly ProfileViewModel _viewModel = App.Locator.ProfileViewModel;
        public Profile()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
        public Profile(User user)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.UserProfile = user;
            _viewModel.Initialize(user);
        }
      
        public Profile(MasterViewModel masterViewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel;
            _viewModel.MasterViewModel = masterViewModel;
            _viewModel.UserProfile = masterViewModel.User; //user
            if (masterViewModel.User.Role == SealegsUserRole.LockerMemberRole)
            {
                _viewModel.LockerMember = masterViewModel.LockerMemberUser; //locker
                _viewModel.Initialize(_viewModel.LockerMember);
                return;
            }
            if (masterViewModel.User.Role == SealegsUserRole.AdminRole)
            {
                _viewModel.SealegsUser = masterViewModel.SealegsUser; //admin
                _viewModel.Initialize(_viewModel.SealegsUser);
                return;
            }
            _viewModel.Initialize(_viewModel.UserProfile);
        }
    }
}