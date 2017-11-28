using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using FormsToolkit;
using Xamarin.Forms;
using Plugin.Fingerprint.Abstractions;

using Sealegs.Clients.Portable;
using Sealegs.Utils;
using Sealegs.Clients.UI.Pages.Android.Menu;

namespace Sealegs.Clients.UI
{
    public partial class LoginPage : ContentPage
    {
        #region Fields

        const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        readonly ImageSource placeholder;
        private readonly LoginViewModel _viewModel = App.Locator.LoginViewModel;
        private User User;

        #endregion

        #region CTOR (2 overloads)

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            if (Device.OS == TargetPlatform.Android)
                _viewModel.IsFingerScanAvailable = Sealegs.Clients.UI.App.AndroidFingerPrintSupported;
            _viewModel.Initialize();

            BindingContext = _viewModel;

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    CircleImageAvatar.Source = ImageSource.FromFile("profile_generic_big.png");
            //});
            //MessagingService.Current.Subscribe<string>(MessageKeys.LoggedIn, (l, email) =>
            //{
            //    var isValid = (Regex.IsMatch(email, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            //    if (isValid)
            //    {
            //        Device.BeginInvokeOnMainThread(() =>
            //        {
            //            CircleImageAvatar.BorderThickness = 3;
            //            CircleImageAvatar.Source = ImageSource.FromUri(new Uri(Gravatar.GetURL(email)));
            //        });

            //    }
            //});
        }

        public LoginPage(User user)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _viewModel.Initialize();
            if (Device.OS == TargetPlatform.Android)
                _viewModel.IsFingerScanAvailable = Sealegs.Clients.UI.App.AndroidFingerPrintSupported;

            BindingContext = _viewModel;
            User = user;
            Navigation.PushAsync(new Master(user));
        }

        #endregion

        #region Navigate

        private async void Navigate()
        {
            if (User != null)
                await _viewModel.ExecuteAlternateLogin(User);
        }

        #endregion

        #region OnBackButtonPressed

        protected override bool OnBackButtonPressed()
        {
            if (Settings.Current.FirstRun)
                return true;

            return base.OnBackButtonPressed();
        }

        #endregion
    }
}

