using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;
using Plugin.Share;

namespace Sealegs.Clients.Portable
{
    public class AboutViewModel : SettingsViewModel
    {

        public ObservableRangeCollection<Grouping<string, MenuItem>> MenuItems { get; }
        // public ObservableRangeCollection<MenuItem> InfoItems { get; } = new ObservableRangeCollection<MenuItem>();
        public ObservableRangeCollection<MenuItem> AccountItems { get; } = new ObservableRangeCollection<MenuItem>();

        // MenuItem syncItem;
        // MenuItem accountItem;
        MenuItem pushItem;
        IPushNotifications push;

        public AboutViewModel()
        {
            AboutItems.Clear();
            // AboutItems.Add(new MenuItem { Name = "About this app", Icon = "icon_venue.png" });
            push = DependencyService.Get<IPushNotifications>();

            //InfoItems.AddRange(new []
            //    {
            //        new MenuItem { Name = "News", Icon = "icon_venue.png", Parameter="news"},
            //         new MenuItem { Name = "Notifications", Icon = "icon_venue.png", Parameter="notifications"},//binny
            //        new MenuItem { Name = "Feedback & Ratings", Icon = "icon_venue.png", Parameter="feedback-ratings"},
            //        new MenuItem { Name = "Maps", Icon = "icon_venue.png", Parameter = "maps"},
            //        new MenuItem { Name = "Restaurant Floor Plans", Icon = "icon_venue.png", Parameter = "restaurant-maps"},
            //        new MenuItem { Name = "Sealegs Promise", Icon = "icon_code_of_conduct.png", Parameter="sealegs-promise" },
            //        new MenuItem { Name = "Wi-Fi Information", Icon = "icon_wifi.png", Parameter="wi-fi" },
            //    });

            //accountItem = new MenuItem
            //    {
            //        Name = "Logged in as:"
            //    };

            //syncItem = new MenuItem
            //    {
            //        Name = "Last Sync:"
            //    };

            pushItem = new MenuItem
            {
                Name = "Enable push notifications"
            };

            pushItem.Command = new Command(() =>
            {
                MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
                {
                    Title = "Push Notification",
                    Question = "To enable push notifications, please go into Settings, Tap Notifications, and set Allow Notifications to on.",
                    Positive = "Settings",
                    Negative = "Maybe Later",
                    OnCompleted = (result) =>
                    {
                        if (result)
                        {
                            push.OpenSettings();
                        }
                    }
                });

                push.RegisterForNotifications();
                pushItem.Name = "Push notifications enabled";
                return;
            });

            // UpdateItems();

            // AccountItems.Add(accountItem);
            // AccountItems.Add(syncItem);
            AccountItems.Add(pushItem);

            //This will be triggered wen 
            //Settings.PropertyChanged += (sender, e) => 
            //    {
            //        if(e.PropertyName == "Email" || e.PropertyName == "LastSync" || e.PropertyName == "PushNotificationsEnabled")
            //        {
            //            UpdateItems();
            //            RaisePropertyChanged("AccountItems");
            //        }
            //    };
        }

        public void UpdateItems()
        {
            //  syncItem.Subtitle = LastSyncDisplay;
            //  accountItem.Subtitle = Settings.Current.IsLoggedIn ? Settings.Current.UserDisplayName : "Not signed in";

            pushItem.Name = push.IsRegistered ? "Push notifications enabled" : "Enable push notifications";
        }

    }
}

