using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FormsToolkit;
using Xamarin.Forms;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MoreLinq;
using MvvmHelpers;

using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class AddEditNotificationsViewModel : BaseViewModel
    {
        #region Global Variables
        private INavigationService _navService;
        #endregion

        #region CTOR
        public AddEditNotificationsViewModel(INavigationService navigation)
        {

            _navService = navigation;

        }
        #endregion

        #region Initialize

        public async void Initialize()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var list = await RoleDb.GetAllRoles();
                if (list.Any())
                {
                    Roles = new ObservableRangeCollection<SealegsUserRole>(list);
                    if (!String.IsNullOrEmpty(Notification.RoleId))
                        FlagSelectedRoles(Notification.RoleId);
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "InitlizeRoleList");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion 

        #region Relay Commands

        public RelayCommand CancelCommand => new RelayCommand(OnCancelCick);

        public RelayCommand SaveCommand => new RelayCommand(OnSaveCick);

        #endregion

        #region Observable Properties

        private Notification _notification;

        public Notification Notification
        {
            get => _notification;
            set
            {
                _notification = value;
                RaisePropertyChanged();
            }
        }

        private ObservableRangeCollection<SealegsUserRole> _roles = new ObservableRangeCollection<SealegsUserRole>();
        public ObservableRangeCollection<SealegsUserRole> Roles
        {
            get => _roles;
            set
            {
                _roles = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Event Handlers & Helpers

        private void OnCancelCick()
        {
            (_navService as ISealegsNavigationService).PopupGoBack();
        }

        #region OnSaveCick

        private async void OnSaveCick()
        {
            if (IsBusy) return;
            bool result = false;
            try
            {
                IsBusy = true;

                Toast.SendToast($"{ButtonName} notification ...");
                var roles = Roles.Where(r => r.IsSelected).Select(s => s.RoleName).ToList();
                var x = Notification.RoleId = string.Join(",", roles);

                if (ButtonName == "Update")
                    result = await NotificationsDb.UpdateNotification(Notification);
                else
                    result = await NotificationsDb.InsertNotification(Notification);
                if (result)
                {
                    await UserDialogs.Instance.AlertAsync($"{Notification.Text} {ButtonName}d!");
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Sorry something went wrong during wine {Notification.Text} {ButtonName}d!");
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "OnSaveCick");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
                (_navService as ISealegsNavigationService).PopupGoBack();
            }
        }

        #endregion 

        #region ItemSelected

        public void ItemSelected(SealegsUserRole role)
        {
            role.IsSelected = !role.IsSelected;
        }

        #endregion

        #region FlagSelectedRoles

        private void FlagSelectedRoles(string roleNames)
        {
            Roles.Where(r => roleNames.Split(',').Any(name => name == r.RoleName)).ForEach(r => r.IsSelected = true);
        }

        #endregion

        #endregion 
    }
}
