using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MvvmHelpers;
using Xamarin.Forms;
using FormsToolkit;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.Fingerprint.Abstractions;
using static Xamarin.Forms.TargetPlatform;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class AddEditLockersViewModel : BaseViewModel
    {
        #region Fields

        private readonly INavigationService _navService;

        #endregion

        #region CTOR

        public AddEditLockersViewModel(INavigationService navigationService)
        {
            _navService = navigationService;
        }

        #endregion

        #region Relay Commands

        public RelayCommand SaveLockerCommand => new RelayCommand(SaveLocker_OnClick);
        public RelayCommand CancelCommand => new RelayCommand(Cancel_OnClick);

        #endregion

        #region Observable Properties

        #region AllLockerTypes 

        ObservableRangeCollection<LockerType> _allLockerTypes;
        public ObservableRangeCollection<LockerType> AllLockerTypes
        {
            get => _allLockerTypes;
            set
            {
                _allLockerTypes = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region SelectedLockerType

        private LockerType _selectedLockerType;
        public LockerType SelectedLockerType
        {
            get => _selectedLockerType;
            set
            {
                _selectedLockerType = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region LockerMember

        private LockerMember _locker;
        public LockerMember Locker
        {
            get => _locker;
            set
            {
                _locker = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region PageName

        private string _pageName;
        public string PageName
        {
            get => _pageName;
            set
            {
                _pageName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Password

        private string _password;
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

        #region IsEmailEnabled

        private bool _isEmailEnabled = true;
        public bool IsEmailEnabled
        {
            get => _isEmailEnabled;
            set
            {
                _isEmailEnabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region IsPasswordEnabled

        private bool _isPasswordEnabled = true;
        public bool IsPasswordEnabled
        {
            get => _isPasswordEnabled;
            set
            {
                _isPasswordEnabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        #region SaveLocker_OnClick

        private async void SaveLocker_OnClick()
        {
            bool result = false;
            try
            {
                IsBusy = true;
                await Task.Delay(200);
                Toast.SendToast($"Saving locker {Locker.DisplayName}");
                Locker.LockerTypeID = SelectedLockerType.LockerTypeID.ToString();
                if (ButtonName == "Update")
                {
                    Locker.ProfileImage = LockerMemberDb.GetBaseImageName(Locker.ProfileImage);
                    result = await LockerMemberDb.UpdateLocker(Locker);
                }
                else
                {
                    var id = Guid.NewGuid();
                    SealegsUser user = new SealegsUser() { Id = id.ToString(), UserID = id, Email = Locker.EmailAddress, FirstName = Locker?.MemberName, LastName = Locker?.DisplayName, IsAnnonymous = false, IsApproved = true, Password = Password, Role = new SealegsUserRole() { Id = SealegsUserRole.LockerMember.ToString() } };
                    var auth = DependencyService.Get<IAuth>();
                    result = await auth.Insert(user);
                    if (!result)
                    {
                        await UserDialogs.Instance.AlertAsync($"Sorry something went wrong during locker user {Locker.MemberName} creation!");
                        return;
                    }
                    Locker.LockerMemberID = Locker.Id = Locker.UserID = id.ToString();
                    result = await LockerMemberDb.InsertLocker(Locker);
                }
                if (result)
                {
                    MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                    {
                        Title = $"Locker {ButtonName}d",
                        Message = $"Locker {Locker.DisplayName} has been {ButtonName}d!",
                        Cancel = "OK"
                    });
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Sorry something went wrong during locker {Locker.DisplayName} {ButtonName}d!");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "SaveLocker_OnClick");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                if(result) MessagingService.Current.SendMessage((ButtonName == "Update") ? MessageKeys.UpdateLocker : MessageKeys.InsertLocker, Locker);
                IsBusy = false;
                (_navService as ISealegsNavigationService).PopupGoBack();
            }
        }

        #endregion

        private void Cancel_OnClick()
        {
            (_navService as ISealegsNavigationService).PopupGoBack();
        }

        #endregion
    }
}
