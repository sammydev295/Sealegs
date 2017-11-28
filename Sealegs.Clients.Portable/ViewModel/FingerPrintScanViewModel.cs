using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Sealegs.Clients.Portable.Locator;
using Sealegs.DataObjects;
using Sealegs.Utils;
using Plugin.Fingerprint.Abstractions;

namespace Sealegs.Clients.Portable.ViewModel
{
    public class FingerPrintScanViewModel : BaseViewModel
    {
        #region Global
        private CancellationTokenSource _cancel;
        private readonly INavigationService _navService;
        #endregion

        #region CTOR
        public FingerPrintScanViewModel(INavigationService navService)
        {
            _navService = navService;
        }
        #endregion

        #region Observable Properties

        public  User CurrentUser { get; set; }
        #endregion

        #region Relay Command
        public RelayCommand LoginCommand=>new RelayCommand(Login_OnClick);
        public RelayCommand FingerScanCommand => new RelayCommand(FingerScan_OnClick);
        #endregion

        #region Events

        private async void FingerScan_OnClick()
        {
            await AuthenticateAsync("Confirm your id by finger scan");
        }
        private void Login_OnClick()
        {
            _navService.NavigateTo(ViewModelLocator.LoginPage);
        }

        public async Task AuthenticateAsync(string reason, string cancel = null, string fallback = null)
        {
            try
            {
                _cancel = new CancellationTokenSource();
                //lblStatus.Text = "";

                var dialogConfig = new AuthenticationRequestConfiguration(reason)
                { // all optional
                    CancelTitle = cancel,
                    FallbackTitle = fallback,
                    AllowAlternativeAuthentication = true
                };

                var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync(dialogConfig, _cancel.Token);

                await SetResultAsync(result);
            }
            catch (Exception e)
            {

            }

        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            if (result.Authenticated)
            {
                _navService.NavigateTo(ViewModelLocator.Master, CurrentUser);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Invalid pattern detected !");
            }
        }
        #endregion

        public void InitlizeUser(User user)
        {
                User = new SealegsUser
                {
                    UserID = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = new SealegsUserRole { RoleName = user.Role }
                };
        }
    }
}
