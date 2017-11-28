using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public partial class FeedbackPage : ContentPage
    {
        #region ViewModel

        FeedbackViewModel ViewModel=App.Locator.FeedbackViewModel;

        #endregion

        #region CTOR (3 Overloads)

        public FeedbackPage(FeaturedEvent featuredEvent)
        {
            InitializeComponent();

            BindingContext = ViewModel;
            if (Device.OS != TargetPlatform.iOS)
                ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(async () =>
            {
                //if (vm.IsBusy)
                //    return;

                await Navigation.PopModalAsync();
            });
        }

        public FeedbackPage(LockerMember locker)
        {
            InitializeComponent();

            BindingContext = ViewModel;
            if (Device.OS != TargetPlatform.iOS)
                ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(async () => 
                {
                    //if(vm.IsBusy)
                    //    return;
                    
                    await Navigation.PopModalAsync();
                });
        }

        public FeedbackPage(Wine wine)
        {
            InitializeComponent();

            BindingContext = ViewModel;
            if (Device.OS != TargetPlatform.iOS)
                ToolbarDone.Icon = "toolbar_close.png";

            ToolbarDone.Command = new Command(async () =>
            {
                //if (vm.IsBusy)
                //    return;

                await Navigation.PopModalAsync();
            });
        }

        #endregion

        #region Event Overrides

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var items = StarGrid.Behaviors.Count;
            for(int i = 0; i < items; i++)
                StarGrid.Behaviors.RemoveAt(i);
        }

        #endregion
    }
}

