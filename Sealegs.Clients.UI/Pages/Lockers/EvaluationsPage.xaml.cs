using System;
using System.Collections.Generic;

using FormsToolkit;
using Xamarin.Forms;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;

namespace Sealegs.Clients.UI
{
    public partial class EvaluationsPage : ContentPage
    {
        private EvaluationsViewModel ViewModel = App.Locator.EvaluationsViewModel;

        public EvaluationsPage ()
        {
            InitializeComponent ();
            BindingContext = ViewModel;
            ListViewLockers.ItemTapped += (sender, e) => ListViewLockers.SelectedItem = null;
            ListViewLockers.ItemSelected +=  (sender, e) => 
            {
                var locker = ListViewLockers.SelectedItem as LockerMember;
                if (locker == null)
                    return;

                if (!Settings.Current.IsLoggedIn) 
                {
                    DependencyService.Get<ILogger> ().TrackPage (AppPage.Login.ToString (), "Feedback");
                    MessagingService.Current.SendMessage (MessageKeys.NavigateLogin);
                    return;
                }
              //  await NavigationService.PushModalAsync (Navigation, new SealegsNavigationPage (new FeedbackPage (locker)));
                ListViewLockers.SelectedItem = null;
            };
        }

        protected override void OnAppearing ()
        {
            base.OnAppearing ();

            if (Device.OS == TargetPlatform.Android)
                MessagingService.Current.Subscribe ("eval_finished", (d) => UpdatePage ());


            UpdatePage ();
        }

        void UpdatePage ()
        {
            //Load if none, or if 45 minutes has gone by
            //if ((viewModel?.Lockers?.Count ?? 0) == 0 || EvaluationsViewModel.ForceRefresh) {
            //    viewModel?.LoadLockersCommand?.Execute (null);
            }

          //  EvaluationsViewModel.ForceRefresh = false;
       // }

        protected override void OnDisappearing ()
        {
            base.OnDisappearing ();

            //if (Device.OS == TargetPlatform.Android)
            //    MessagingService.Current.Unsubscribe ("eval_finished");
        }
    }
}

