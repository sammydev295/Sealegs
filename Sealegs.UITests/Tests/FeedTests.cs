using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Sealegs.UITests
{
        public class FeedTests : AbstractSetup
        {
                public FeedTests(Platform platform)
                    : base(platform)
                {
                    }

                [Test]
                public void AnnouncementVerify()
                {
                        new FeedPage()
                            .SelectAnnouncement();

                        new NotificationsPage()
                            .SelectAnnouncementItem();


                    }

                [Test]
                public void SocialVerify()
                {
                        new FeedPage()
                            .SelectSocialItem();
                    }

                [Test]
                public void FavoriteVerify()
                {
                        if (OnAndroid)
                        {
                            new FeedPage()
                                .NavigateTo("Settings");

                            new SettingsPage()
                                .TapSignIn();

                            new LoginPage()
                                .EnterCredentials("xtc@xamarin.com", "fake")
                                .TapLogin();

                            new SettingsPage()
                                .ConfirmedLoggedIn()
                                .NavigateTo("Lockers");
                
                        }
                        if (OniOS)
                        {
                            new FeedPage()
                                .NavigateTo("Info");

                            new InfoPage()
                                .TapSignIn();

                            new LoginPage()
                                .EnterCredentials("xtc@xamarin.com", "fake")
                                .TapLogin();

                            new InfoPage()
                                .ConfirmedLoggedIn()
                                .NavigateTo("Lockers");
                        }

                        
                        new LockersPage()
                            .FavoriteFirstLocker();

                        new LockersPage()
                            .NavigateTo("Evolve Feed");

                        new FeedPage()
                            .SelectFavorite();

                        new LockerDetailsPage();

                    }
            }
}
