using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Xamarin.Forms;

using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        private INavigationService _navService;

        #region CTOR

        public AddEmployeeViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Initialize

        public void Initialize()
        {
            FullName = Locker.DisplayName;
            Email = Locker.EmailAddress;
            Mobile = Locker.Mobile;
            Twitter = Locker.TwitterID;
            Facebook = Locker.FacebookID;
            Notes = Locker.Notes;
        }

        #endregion

        #region Observable Properties
        private string _buttonName;
        public string ButtonName
        {
            get => _buttonName;
            set
            {
                _buttonName = value;
                RaisePropertyChanged();
            }
        }

        #region PageTitle
        private string _pageTitle = String.Empty;
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Full Name
        private string _fullName = String.Empty;
        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                RaisePropertyChanged();
                Locker.DisplayName = FullName;
            }
        }
        #endregion

        #region Email 
        private string _email = String.Empty;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged();
                Locker.EmailAddress = Email;
            }
        }
        #endregion

        #region Password 
        private string _password = String.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ConfirmPassword 
        private string _confirmPassword = String.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Mobile 
        private string _mobile = String.Empty;
        public string Mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                RaisePropertyChanged();
                Locker.Mobile = Mobile;
            }
        }
        #endregion 

        #region Twitter 
        private string _twitter = String.Empty;
        public string Twitter
        {
            get => _twitter;
            set
            {
                _twitter = value;
                RaisePropertyChanged();
                Locker.TwitterID = Twitter;
            }
        }
        #endregion 

        #region Facebook 
        private string _facebook = String.Empty;
        public string Facebook
        {
            get => _facebook;
            set
            {
                _facebook = value;
                RaisePropertyChanged();
                Locker.FacebookID = Facebook;
            }
        }
        #endregion 

        #region Notes 
        private string _notes = String.Empty;
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private LockerMember _locker = new LockerMember();
        public LockerMember Locker
        {
            get => _locker;
            set
            {
                _locker = value;
                RaisePropertyChanged();
                Initialize();
            }
        }
        #endregion

        #region Relay Commands

        public RelayCommand AddEmployeeCommand => new RelayCommand(AddEmployee_OnClick);
        public RelayCommand ResetFieldCommand => new RelayCommand(ResetFields_OnClick);

        #endregion

        #region Event Handlers

        private void AddEmployee_OnClick()
        {
            if (ButtonName == "Update")
            {
                LockerMemberDb.UpdateLocker(Locker);
                return;
            }
            LockerMemberDb.InsertLocker(Locker);
        }

        private void ResetFields_OnClick()
        {
            FullName = String.Empty;
            Email = String.Empty;
            Mobile = String.Empty;
            Twitter = String.Empty;
            Facebook = String.Empty;
            Notes = String.Empty;
        }

        #endregion

    }
}
