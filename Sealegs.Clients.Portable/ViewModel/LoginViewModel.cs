using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;

using FormsToolkit;
using Xamarin.Forms;

using Acr.UserDialogs;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Command;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable.Locator;
using Sealegs.Clients.Portable.ViewModel;
using Sealegs.Utils;
using Sealegs.DataStore.Azure;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.Clients.Portable
{
    public class LoginViewModel : BaseViewModel
    {
        #region Fields

        private CancellationTokenSource _cancel;

        private readonly IAuth _auth;
        private readonly INavigationService _navigationService;

        #endregion

        #region CTOR

        public LoginViewModel(INavigationService navigation)
        {
            _navigationService = navigation;
            _auth = DependencyService.Get<IAuth>();
        }

        #endregion

        #region Initialize

        public async Task Initialize()
        {
            User = null;
            ProfilePath = ImageSource.FromFile("profile_generic_big.png");
            //Enable Hardware>Touch Id>Tougle Enrolled State in MAC

            if (Device.OS == TargetPlatform.iOS)
                IsFingerScanAvailable = await CrossFingerprint.Current.IsAvailableAsync();

            if (!IsFingerScanAvailable)
                return;

            // User from local Settings
            var settings = Sealegs.Clients.Portable.ViewModel.BaseViewModel.UserTable;
            CurrentUser = settings.GetUser();
            IsFingerScanAvailable = (CurrentUser != null);

            if (CurrentUser != null && CurrentUser.IsFingerScanEnable)
                FingerScan_OnClick();
        }

        #endregion

        #region Observable Properties

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        private string _email = "Sammy.dev@drdev.com";//"gus.calderon@drdev.com";//
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged();
            }
        }

        private string _password = "Setup295!";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }

        bool _isFingerScanAvailable = false;
        public bool IsFingerScanAvailable
        {
            get => _isFingerScanAvailable;
            set
            {
                _isFingerScanAvailable = value;
                RaisePropertyChanged();
            }
        }

        String _scanStatusSourceText = "Initiate FingerPrint Scan";
        public String ScanStatusSourceText
        {
            get => _scanStatusSourceText;
            set
            {
                _scanStatusSourceText = value;
                RaisePropertyChanged();
            }
        }

        ImageSource _scanStatusSourceImage = ImageSource.FromFile("fingerprint_red.png");
        public ImageSource ScanStatusSourceImage
        {
            get => _scanStatusSourceImage;
            set
            {
                _scanStatusSourceImage = value;
                RaisePropertyChanged();
            }
        }

        #region ProfilePath

        private ImageSource _profilePath;
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

        public User CurrentUser { get; set; }

        #endregion

        #region Relay Commands

        public RelayCommand LoginCommand => new RelayCommand(async () => await ExecuteLoginAsync());
        public RelayCommand SignupCommand => new RelayCommand(async () => await ExecuteSignupAsync());
        public RelayCommand CancelCommand => new RelayCommand(ExecuteCancelAsync);
        public RelayCommand FingerScanCommand => new RelayCommand(FingerScan_OnClick);

        #endregion

        #region Events Handler Helper Methods

        #region ExecuteLoginAsync

        private async Task ExecuteLoginAsync()
        {
            if (IsBusy)
                return;

            if (string.IsNullOrWhiteSpace(Email))
            {
                ShowMessage("Sign in Information", "We do need your email address :-)");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ShowMessage("Sign in Information", Message = "Password is empty!");
                return;
            }

            try
            {
                IsBusy = true;
                Message = "Signing in...";
                await Task.Delay(1000);

                var result = await _auth.LoginAsync(Email, Password);
                if (result == null) return;

                Response = AccountFromMobileServiceUser(result);
                if (Response.Success)
                {
                    // Setting User
                    FillUser(Response);

                    // Clear settings database 
                    UserDb.DeleteAll();

                    // Insert User In settings sataBase
                    UserDb.InsertUser(Response.User);

                    _navigationService.NavigateTo(ViewModelLocator.Master, Response.User);
                }
            }
            catch (Exception ex)
            {
                Logger.Track(SealegsLoggerKeys.LoginFailure, "Reason", ex?.Message ?? string.Empty);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Unable to Sign in",
                    Message = "The email or password provided is incorrect.",
                    Cancel = "OK"
                });
            }
            finally
            {
                Message = string.Empty;
                IsBusy = false;
                ProfilePath = ImageSource.FromUri(new Uri(Gravatar.GetURL(Email)));
                MessagingService.Current.SendMessage(MessageKeys.LoggedIn, User.Role.RoleName);
            }
        }

        #endregion 

        #region ExecuteSignupAsync

        private async Task ExecuteSignupAsync()
        {
            Logger.Track(SealegsLoggerKeys.Signup);
            await CrossShare.Current.OpenBrowser("https://auth.xamarin.com/account/register",
                new BrowserOptions
                {
                    ChromeShowTitle = true,
                    ChromeToolbarColor = new ShareColor
                    {
                        A = 255,
                        R = 118,
                        G = 53,
                        B = 235
                    },
                    UseSafariWebViewController = true
                });
        }

        #endregion 

        private void ExecuteCancelAsync() => _navigationService.NavigateTo(ViewModelLocator.Master);

        public async Task ExecuteAlternateLogin(User user)
        {
            _navigationService.NavigateTo(ViewModelLocator.Master, user);

        }

        #endregion

        #region FillUser (2 overloads)

        private static void FillUser(AccountResponse response)
        {
            User = new SealegsUser
            {
                UserID = response.User.Id,
                Email = response.User.Email,
                FirstName = response.User.FirstName,
                LastName = response.User.LastName,
                Role = Sealegs.DataObjects.SealegsUserRole.RoleHierarchy.ToList().Find(r => r.RoleName == response.User.Role)
            };

        }

        private void FillUser(User user)
        {
            User = new SealegsUser
            {
                UserID = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = Sealegs.DataObjects.SealegsUserRole.RoleHierarchy.ToList().Find(r => r.RoleName == user.Role)
            };

            // VEry important to do this
            Addresses.Token = user.Token;
        }

        #endregion

        #region ShowMessage

        private static void ShowMessage(string title, string message, string cancel = "OK")
        {
            MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
            {
                Title = title,
                Message = message,
                Cancel = cancel
            });
        }

        #endregion

        #region AccountFromMobileServiceUser

        private AccountResponse AccountFromMobileServiceUser(MobileServiceUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IDictionary<string, string> claims = JwtUtility.GetClaims(user.MobileServiceAuthenticationToken);

            var account = new AccountResponse();
            account.Success = true;
            account.User = new User
            {
                //Id = Guid.Parse(user.UserId),
                Id = Guid.Parse(claims[JwtClaimNames.Subject]),
                FirstName = claims[JwtClaimNames.GivenName],
                LastName = claims[JwtClaimNames.FamilyName],
                Email = claims[JwtClaimNames.Email],
                Role = claims[JwtClaimNames.Role],
                Token = user.MobileServiceAuthenticationToken,
                IsFingerScanEnable = true
            };
            account.Token = user.MobileServiceAuthenticationToken;

            return account;
        }

        #endregion

        #region FingerPrint Auth

        public async void FingerScan_OnClick()
        {
            await AuthenticateAsync("Confirm your id by a finger scan");
        }

        public async Task AuthenticateAsync(string reason, string cancel = null, string fallback = null)
        {
            try
            {
                _cancel = new CancellationTokenSource();
                var dialogConfig = new AuthenticationRequestConfiguration(reason)
                { // all optional
                    CancelTitle = cancel,
                    FallbackTitle = fallback,
                    AllowAlternativeAuthentication = false
                };

                var result = await Plugin.Fingerprint.CrossFingerprint.Current.AuthenticateAsync(dialogConfig, _cancel.Token);
                await SetResultAsync(result);
            }
            catch (Exception ex)
            {
                Logger.Track(SealegsLoggerKeys.LoginFailure, "Reason", ex?.Message ?? string.Empty);
                MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.Message, new MessagingServiceAlert
                {
                    Title = "Fingerprint Scan Issue",
                    Message = ex?.Message,
                    Cancel = "OK"
                });
            }
        }

        private async Task SetResultAsync(FingerprintAuthenticationResult result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (result.Authenticated)
                {
                    //ScanStatusSourceImage = ImageSource.FromFile("fingerprint_green.png");
                    ScanStatusSourceText = "FingerPrint Scan Successful";
                }
                else
                {
                    //ScanStatusSourceImage = ImageSource.FromFile("fingerprint_red.png");
                    ScanStatusSourceText = (result.Status == FingerprintAuthenticationResultStatus.Canceled) ? "Confirm your id by a finger scan" : "FingerPrint Scan Failed, try again!";
                }
            });

            if (result.Authenticated)
            {
                FillUser(CurrentUser);
                await ExecuteLoginAsync();
            }
        }

        #endregion
    }
}

