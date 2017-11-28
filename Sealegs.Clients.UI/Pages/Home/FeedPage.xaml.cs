using System;
using System.Collections.Generic;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.DataObjects;
using Sealegs.Clients.Portable;

namespace Sealegs.Clients.UI
{
    public partial class FeedPage : ContentPage
    {
        private FeedViewModel ViewModel = App.Locator.FeedViewModel;
        FeedViewModel vm;
        DateTime favoritesTime;
        string loggedIn;
        public FeedPage()
        {
            InitializeComponent();
            loggedIn = Settings.Current.Email;
            BindingContext = ViewModel;

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Refresh",
                    Icon = "toolbar_refresh.png",
                    Command = ViewModel.RefreshCommand
                });
            }

            favoritesTime = Settings.Current.LastFavoriteTime;
            ViewModel.Tweets.CollectionChanged += (sender, e) => 
                {
                    var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Tweets.Count + 2;
                    ListViewSocial.HeightRequest = (ViewModel.Tweets.Count * ListViewSocial.RowHeight)  - adjust;
                };

            NotificationStack.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command( () => 
                        {
                            App.Logger.TrackPage(AppPage.Notification.ToString());
                         //   await NavigationService.PushAsync(Navigation, new NotificationsPage());
                        })
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();

            MessagingService.Current.Subscribe<string>(MessageKeys.NavigateToImage,  (m, image) =>
                {
                    App.Logger.TrackPage(AppPage.TweetImage.ToString(), image);
                   // await NavigationService.PushModalAsync(Navigation, new SealegsNavigationPage(new TweetImagePage(image)));
                });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingService.Current.Unsubscribe<string>(MessageKeys.NavigateToImage);
        }

        bool firstLoad = true;
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow)) ||
                    loggedIn != Settings.Current.Email;

            loggedIn = Settings.Current.Email;
            if (forceRefresh)
            {
                ViewModel.RefreshCommand.Execute(null);
                favoritesTime = Settings.Current.LastFavoriteTime;
            }
            else
            {
                if (ViewModel.Tweets.Count == 0)
                {
                    ViewModel.LoadSocialCommand.Execute(null);
                }

                if (favoritesTime != Settings.Current.LastFavoriteTime)
                {
                    if (firstLoad)
                        Settings.Current.LastFavoriteTime = DateTime.UtcNow;
                    
                    firstLoad = false;
                    favoritesTime = Settings.Current.LastFavoriteTime;
                }

                if (ViewModel.Notification == null)
                    ViewModel.LoadNotificationsCommand.Execute(null);
            }
        }

        public void OnResume()
        {
            UpdatePage();
        }

    }
}

