using System;

using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;

using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;
using Sealegs.Utils;
using Xamarin.Forms;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class MasterViewModel : BaseViewModel
    {
        #region Fields

        private readonly INavigationService _navService;

        #endregion

        #region CTOR

        public MasterViewModel(INavigationService navService)
        {
            _navService = navService;
        }

        #endregion

        #region Relay Commands
        public RelayCommand UserSignInCommand => new RelayCommand(UserSignIn_OnClick);

        public RelayCommand ProfileCommand => new RelayCommand(Profile_OnClick);
        #endregion

        #region Observable Properties

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged();
            }
        }

        public SealegsUser SealegsUser { get; set; }

        private string _username = String.Empty;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged();
            }
        }

        private ImageSource _profilePath = String.Empty;
        public ImageSource ProfilePath
        {
            get => _profilePath;
            set
            {
                _profilePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Event Handlers & Helpers

        public async void CheckUserRole()
        {
            if (User.Role == SealegsUserRole.LockerMemberRole)
            {
                LockerMemberUser = await LockerMemberDb.GetByMemberId(User.Id.ToString());
                if (LockerMemberUser != null)
                {
                    Username = LockerMemberUser.DisplayName;
                    ProfilePath = LockerMemberUser.ProfileImage;
                }
            }

            if (User.Role == SealegsUserRole.AdminRole)
            {
                 SealegsUser = await DependencyService.Get<IAuth>().GetUser(User.Id.ToString());
                if (SealegsUser != null)
                {
                    Username = SealegsUser.FirstName;
                    ProfilePath = SealegsUser.AvatarImage;
                }
            }
        }

        private void Profile_OnClick()
        {
            _navService.NavigateTo(ViewModelLocator.Profile, this);
        }

        private void UserSignIn_OnClick()
        {
            if (Username == "Sign In")
            {
                _navService.GoBack();
            }
        }

        public void Logout()
        {
            User = null;
            ProfilePath = null;
            Username  = String.Empty;
            _navService.GoBack();
        }

        #endregion
    }
}
