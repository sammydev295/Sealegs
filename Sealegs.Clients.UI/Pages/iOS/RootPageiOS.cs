using Xamarin.Forms;
using FormsToolkit;

using Sealegs.Clients.Portable;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.Clients.UI
{
    public class RootPageiOS : TabbedPage
    {
        public RootPageiOS()
        {

            #if ENABLE_TEST_CLOUD
            if (Settings.Current.Email == "xtc@xamarin.com")
            {
                Settings.Current.FirstRun = true;
                Settings.Current.FirstName = string.Empty;
                Settings.Current.LastName = string.Empty;
                Settings.Current.Email = string.Empty;
            }
            #endif

            NavigationPage.SetHasNavigationBar(this, false);
            Children.Add(new SealegsNavigationPage(new AboutPage()));
            Children.Add(new SealegsNavigationPage(new LockersPage()));
            Children.Add(new SealegsNavigationPage(new Employees()));
            Children.Add(new SealegsNavigationPage(new MiniHacksPage()));
            Children.Add(new SealegsNavigationPage(new EventsPage()));
            Children.Add(new SealegsNavigationPage(new FeedPage()));

            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
                {
                    switch(p.Page)
                    {
                        case AppPage.Notification:
                            NavigateAsync(AppPage.Notification);
                            await CurrentPage.Navigation.PopToRootAsync();
                            await CurrentPage.Navigation.PushAsync(new NotificationsPage());
                            break;
                        case AppPage.Events:
                            NavigateAsync(AppPage.Events);
                            await CurrentPage.Navigation.PopToRootAsync();
                            break;
                        case AppPage.MiniHacks:
                            NavigateAsync(AppPage.MiniHacks);
                            await CurrentPage.Navigation.PopToRootAsync();
                            break;
                        case AppPage.Lockers:
                            NavigateAsync(AppPage.Lockers);
                            var locker = await DependencyService.Get<ILockerMemberStore>().GetSingleLockerMember(1);
                            if (locker == null)
                                break;
                            //await CurrentPage.Navigation.PushAsync(new LockerDetailsPage(locker));
                            break;
                    }
                });
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            switch (Children.IndexOf(CurrentPage))
            {
                case 0:
                    App.Logger.TrackPage(AppPage.Feed.ToString());
                    break;
                case 1:
                    App.Logger.TrackPage(AppPage.Lockers.ToString());
                    break;
                case 2:
                    App.Logger.TrackPage(AppPage.Events.ToString());
                    break;
                case 3:
                    App.Logger.TrackPage(AppPage.MiniHacks.ToString());
                    break;
                case 4:
                    App.Logger.TrackPage(AppPage.Information.ToString());
                    break;
            }
        }

        public void NavigateAsync(AppPage menuId)
        {
            switch ((int)menuId)
            {
                case (int)AppPage.Feed: CurrentPage = Children[0]; break;
                case (int)AppPage.Lockers: CurrentPage = Children[1]; break;
                case (int)AppPage.Events: CurrentPage = Children[2]; break;
                case (int)AppPage.MiniHacks: CurrentPage = Children[3]; break;
                case (int)AppPage.Information: CurrentPage = Children[4]; break;
                case (int)AppPage.Notification: CurrentPage = Children[0]; break;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Settings.Current.FirstRun)
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
        }
    }
}


