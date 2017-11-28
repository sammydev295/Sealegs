using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;

using Acr.UserDialogs;
using Xamarin.Forms;
using MvvmHelpers;
using FormsToolkit;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.Share;

using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class WineDetailViewModel : BaseViewModel
    {
        #region CTOR

        public INavigationService _navService;
        public WineDetailViewModel(INavigationService navigation)
        {
            _navService = navigation;
        }

        #endregion

        #region Properties

        private bool _buttonVisibility;
        public bool ButtonVisibility
        {
            get => _buttonVisibility;
            set
            {
                _buttonVisibility = value;
                RaisePropertyChanged();
            }
        }

        private bool _checkedOutDateVisibility;
        public bool CheckOutDateVisibility
        {
            get => _checkedOutDateVisibility;
            set
            {
                _checkedOutDateVisibility = value;
                RaisePropertyChanged();
            }
        }

        private Wine _wine;
        public Wine Wine
        {
            get => _wine;
            set
            {
                _wine = value;
                RaisePropertyChanged();
                ButtonName = (!Wine.IsChilledRequestSent) ? "Send Chill Request" : "Chilled Request Sent";
                CheckOutDateVisibility = Wine.IsChecked.Value;
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Relay Commands

        public RelayCommand ChillRequestCommand => new RelayCommand(ChillRequest_OnClick);

        #endregion

        #region Events Handlers Helper Methods

        private async void ChillRequest_OnClick()
        {
            try
            {
                Toast.SendToast($"Sending Chill request for {Wine.WineTitle} ...");
                var chillRequest = new RemoteChillRequest()
                {
                    LockerMemberID = Wine.LockerID,
                    RequestDate = DateTime.Now,
                    MemberBottleID = Wine.MemberBottleID,
                    RemoteChillRequestID = Guid.NewGuid().ToString(),
                    IsCompleted = false

                };
                IsBusy = true;

                var result = await RemoteChilledRequestDb.InsertRemoteChillRequest(chillRequest);
                if (result)
                {
                    await UserDialogs.Instance.AlertAsync($"Chill request text messsages sent for {Wine.WineTitle}");
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync($"Sorry something went wrong sending chill request for {Wine.WineTitle} :( Please try again later!");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogger>().Report(ex, "Method", "ChillRequest_OnClick");
                MessagingService.Current.SendMessage(MessageKeys.Error, ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
