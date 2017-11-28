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
using MvvmHelpers;

using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class NotificationsManagmentViewModel : BaseViewModel
    {
        #region Fields

        private INavigationService navService;

        #endregion

        #region CTOR

        public NotificationsManagmentViewModel(INavigationService navService)
        {
            this.navService = navService;
        }

        #endregion

        #region Initialize

        public async void Initialize()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Toast.SendToast($"Loading notifications ...");
                var list = await NotificationsDb.GetAll();
                if (!list.Any())
                {
                    NoNewsFound = true;
                    ListVisibilty = false;
                }
                else
                {
                    NoNewsFound = false;
                    ListVisibilty = true;
                    NotificationsList = new ObservableRangeCollection<Notification>(list);
                }
            }
            catch (Exception ex)
            {
                Logger.Report(ex, "Method", "Initialize");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region Observable Properties

        private ObservableRangeCollection<Notification> _notificationsList;
        public ObservableRangeCollection<Notification> NotificationsList
        {
            get => _notificationsList;
            set
            {
                _notificationsList = value;
                RaisePropertyChanged();
            }
        }

        private bool _noNewsFound;
        public bool NoNewsFound
        {
            get => _noNewsFound;
            set
            {
                _noNewsFound = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Relay Commands

        public RelayCommand AddCommand => new RelayCommand(AddNotifications);
        
        #endregion

        #region Event Handlers & Helpers

        private void AddNotifications()
        {
            navService.NavigateTo(ViewModelLocator.AddEditNotifications);
        }

        #region HandleSelection

        public async void HandleSelection(Notification notification)
        {
            var result = await UserDialogs.Instance.ActionSheetAsync("Select", "Cancel", null, null, "Edit", "Send", "Delete");
            switch (result)
            {
                case "Edit":
                    (navService as ISealegsNavigationService).PopupNavigateTo(ViewModelLocator.AddEditNotifications, notification);
                    break;

                case "Delete":
                    IsBusy = true;
                    var isDeleted = await NotificationsDb.DeleteNotification(notification.Id);
                    IsBusy = false;
                    if (isDeleted)
                    {
                        NotificationsList.Remove(notification);
                    }
                    break;

                case "Send":
                    IsBusy = true;
                    var isSent = await NotificationsDb.SendNotification(notification);
                    IsBusy = false;
                    break;

                default:
                    break;
            }
        }

        #endregion

        #endregion
    }
}
