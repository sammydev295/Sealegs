using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.Clients.Portable;
using MenuItem = Sealegs.Clients.Portable.MenuItem;

namespace Sealegs.Clients.UI
{
    public class RootPageWindows : MasterDetailPage
    {
        Dictionary<AppPage, Page> pages;
        MenuPageUWP menu;
        public static bool IsDesktop { get; set; }
        public RootPageWindows()
        {
            //MasterBehavior = MasterBehavior.Popover;
            pages = new Dictionary<AppPage, Page>();

            var items = new ObservableCollection<MenuItem>
            {
                new MenuItem { Name = "Evolve Feed", Icon = "menu_feed.png", Page = AppPage.Feed },
                new MenuItem { Name = "Lockers", Icon = "menu_locker.png", Page = AppPage.Lockers },
                new MenuItem { Name = "Events", Icon = "menu_events.png", Page = AppPage.Events },
                new MenuItem { Name = "Mini-Hacks", Icon = "menu_hacks.png", Page = AppPage.MiniHacks },
                new MenuItem { Name = "News", Icon = "menu_news.png", Page = AppPage.News },
                new MenuItem { Name = "Evaluations", Icon = "menu_evals.png", Page = AppPage.Evals },
                new MenuItem { Name = "Venue", Icon = "menu_venue.png", Page = AppPage.Venue },
                new MenuItem { Name = "Floor Maps", Icon = "menu_plan.png", Page = AppPage.FloorMap },
                new MenuItem { Name = "Conference Info", Icon = "menu_info.png", Page = AppPage.ConferenceInfo },
                new MenuItem { Name = "Settings", Icon = "menu_settings.png", Page = AppPage.Settings }
            };

            menu = new MenuPageUWP();
            menu.MenuList.ItemsSource = items;


            menu.MenuList.ItemSelected +=  (sender, args) =>
            {
                if (menu.MenuList.SelectedItem == null)
                    return;

                Device.BeginInvokeOnMainThread( () =>
                {
                    NavigateAsync(((MenuItem)menu.MenuList.SelectedItem).Page);
                    if (!IsDesktop)
                        IsPresented = false;
                });
            };

            Master = menu;
            NavigateAsync((int)AppPage.Lockers);   // Lockers replace with Feed.
            Title ="Sealegs";                        // Xamarin Evolve
        }



        public void NavigateAsync(AppPage menuId)
        {
            Page newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                //only cache specific pages
                switch (menuId)
                {
                    case AppPage.Lockers://lockers
                        pages.Add(menuId, new SealegsNavigationPage(new LockersPage()));
                        break;
                    case AppPage.Feed: //Feed
                        pages.Add(menuId, new SealegsNavigationPage(new FeedPage()));
                        break;
                    case AppPage.Employees://Employees
                        pages.Add(menuId, new SealegsNavigationPage(new Employees()));
                        break;
                    case AppPage.AddEmployee: // AddEmployee
                        pages.Add(menuId,new SealegsNavigationPage(new AddEmployee()));
                        break;
                    case AppPage.Events://events
                        pages.Add(menuId, new SealegsNavigationPage(new EventsPage()));
                        break;
                    case AppPage.MiniHacks://Mini-Hacks
                        newPage = new SealegsNavigationPage(new MiniHacksPage());
                        break;
                    case AppPage.News://news
                        newPage = new SealegsNavigationPage(new NewsPage());
                        break;
                    case AppPage.Evals: //venue
                        newPage = new SealegsNavigationPage(new EvaluationsPage());
                        break;
                    case AppPage.Venue: //venue
                        newPage = new SealegsNavigationPage(new VenuePage());
                        break;
                    case AppPage.ConferenceInfo://Conference info
                        newPage = new SealegsNavigationPage(new ConferenceInformationPage());
                        break;
                    case AppPage.FloorMap://Floor Maps
                        newPage = new SealegsNavigationPage(new FloorMapsCarouselPage());
                        break;
                    case AppPage.Settings://Settings
                        newPage = new SealegsNavigationPage(new SettingsPage());
                        break;
                }
            }

            if(newPage == null)
                newPage = pages[menuId];
            
            if(newPage == null)
                return;

            Detail = newPage;
            //await Navigation.PushAsync(newPage);
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


