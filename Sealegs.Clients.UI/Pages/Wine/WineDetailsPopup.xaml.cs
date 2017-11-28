using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;
using Sealegs.Clients.Portable.ViewModel;

namespace Sealegs.Clients.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WineDetailsPopUp : PopupPage
    {
        #region ViewModel

        private readonly WineDetailViewModel _viewModel = App.Locator.WineDetailViewModel;

        #endregion
         
        #region CTOR

        public WineDetailsPopUp(Wine wine)
        {
            InitializeComponent();

            BindingContext = _viewModel;
            _viewModel.Wine = wine;
        }

        #endregion

        #region Event Overrides

        protected override void OnAppearing()
        {
            base.OnAppearing();

            FrameContainer.HeightRequest = -1;

            CloseImage.Rotation = 30;
            CloseImage.Scale = 0.3;
            CloseImage.Opacity = 0;

            OKButton.Scale = 0.3;
            OKButton.Opacity = 0;

            WineVintage.TranslationX = WineVintage.TranslationX = -10;
            WineVintage.Opacity = WineVintage.Opacity = 0;
        }

        protected async override Task OnAppearingAnimationEnd()
        {
            var translateLength = 400u;

            await Task.WhenAll(
                WineVintage.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                WineVintage.FadeTo(1),
                (new Func<Task>(async () =>
                {
                    await Task.Delay(200);
                    await Task.WhenAll(
                        WineVintage.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                        WineVintage.FadeTo(1));

                }))());

            await Task.WhenAll(
                CloseImage.FadeTo(1),
                CloseImage.ScaleTo(1, easing: Easing.SpringOut),
                CloseImage.RotateTo(0),
                OKButton.ScaleTo(1),
                OKButton.FadeTo(1));
        }

        protected async override Task OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            await Task.WhenAll(
                WineVintage.FadeTo(0),
                OKButton.FadeTo(0));

            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            },
            start: currentHeight,
            end: 170,
            finished: async (d, b) =>
            {
                await Task.Delay(300);
                taskSource.TrySetResult(true);
            });

            await taskSource.Task;
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        #endregion

        #region OnCloseButtonTapped

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        #endregion

        #region CloseAllPopup

        private async void CloseAllPopup()
        {
            (_viewModel._navService as ISealegsNavigationService).PopupGoBack();
        }

        #endregion
    }
}
