using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using FormsToolkit;

using Sealegs.Clients.Portable;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.Clients.UI
{
    public class RootPageAndroid : MasterDetailPage
    {
        Dictionary<int, SealegsNavigationPage> pages;
        DeepLinkPage page;
        bool isRunning = false;
        public RootPageAndroid()
        {
            pages = new Dictionary<int, SealegsNavigationPage>();
            Master = new MenuPage(this);
            pages.Add((int)AppPage.Lockers, new SealegsNavigationPage(new LockersPage()));
            Detail = pages[0];
            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
                {
                    page = p;
                    if(isRunning)
                        await GoToDeepLink();
                });
        }

        public async Task NavigateAsync(int menuId)
        {
            try
            {
                SealegsNavigationPage newPage = null;
                if (!pages.ContainsKey(menuId))
                {
                    //only cache specific pages
                    switch (menuId)
                    {
                        case (int)AppPage.Lockers://lockers
                            pages.Add(menuId, new SealegsNavigationPage(new LockersPage()));
                            break;
                        case (int)AppPage.Employees: //Employees
                            pages.Add(menuId, new SealegsNavigationPage(new Employees()));
                            break;
                        case (int)AppPage.Feed: //Feed
                            pages.Add(menuId, new SealegsNavigationPage(new FeedPage()));
                            break;
                        case (int)AppPage.Events://events
                            pages.Add(menuId, new SealegsNavigationPage(new EventsPage()));
                            break;
                        case (int)AppPage.MiniHacks://Mini-Hacks
                            newPage = new SealegsNavigationPage(new MiniHacksPage());
                            break;
                        case (int)AppPage.News://news
                            newPage = new SealegsNavigationPage(new NewsPage());
                            break;
                        case (int)AppPage.Venue: //venue
                            newPage = new SealegsNavigationPage(new VenuePage());
                            break;
                        case (int)AppPage.ConferenceInfo://Conference info
                            newPage = new SealegsNavigationPage(new ConferenceInformationPage());
                            break;
                        case (int)AppPage.FloorMap://Floor Maps
                            newPage = new SealegsNavigationPage(new FloorMapsCarouselPage());
                            break;
                        case (int)AppPage.Settings://Settings
                            newPage = new SealegsNavigationPage(new SettingsPage());
                            break;
                        case (int)AppPage.Evals:
                            newPage = new SealegsNavigationPage(new EvaluationsPage());
                            break;
                    }
                }


                if (newPage == null)
                    newPage = pages[menuId];

                if (newPage == null)
                    return;

                //if we are on the same tab and pressed it again.
                if (Detail == newPage)
                {
                    await newPage.Navigation.PopToRootAsync();
                }
                Detail = newPage;
            }
            catch (Exception e)
            { }
          
        }

        private async Task PreLoginNavigation()
        {
            
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.Current.FirstRun)
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);

            isRunning = true;

            await GoToDeepLink();
        }

        async Task GoToDeepLink()
        {
            if (page == null)
                return;
            var p = page.Page;
            var id = page.Id;
            page = null;
            switch(p)
            {
                case AppPage.Lockers:
                    await NavigateAsync((int)AppPage.Lockers);
                    var locker = await DependencyService.Get<ILockerMemberStore>().GetSingleLockerMember(1);
                    if (locker == null)
                        break;

                    //await Detail.Navigation.PushAsync(new LockerDetailsPage(locker));
                    break;
            }
        }
    }
}


