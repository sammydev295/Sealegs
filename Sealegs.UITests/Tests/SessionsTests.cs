using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace Sealegs.UITests
{
    public class LockersTests : AbstractSetup
    {
        public LockersTests(Platform platform)
            : base (platform)
        {
        }

        [TestCase("BunchORubbish", false)]
        [TestCase("Xamarin.Forms", true)]
        public void SearchLockers(string search, bool valid)
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .EnterSearchAndVerify(search, valid);
        }

        [Test]
        public void FilterLockers()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .GoToFilterLockersPage();

            new FilterLockersPage()
                .SelectLockerFilters(true, false)
                .SelectLockerCategories(
                    DisplayAll:false,
                    Android:false,
                    iOS:false,
                    XamarinForms:true,
                    Design:false,
                    Secure:true,
                    Test:false,
                    Monitor:false)
                .CloseLockerFilter();

            new LockersPage();
        }

        [Test]
        public void FavoriteLockerSignedOut()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .FavoriteFirstLocker();
            
            new LoginPage()
                .EnterCredentials("xtc@xamarin.com", "fake")
                .TapLogin();

            new LockersPage()
                .ValidateFavorite();
        }

        [Test]
        public void FavoriteLockerSignedIn()
        {
            SignIn();

            new LockersPage()
                .FavoriteFirstLocker();

            new LockersPage()
                .ValidateFavorite();
        }

        [Test]
        public void InvestigateLockerDetails()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .InvestigateFirstLocker();

            new LockerDetailsPage()
                .VerifyContentPresent();
        }

        [Test]
        public void RateLockerSignedOut()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .InvestigateFirstLocker();

            var page = new LockerDetailsPage()
                .VerifyContentPresent()
                .RateThisLocker();

            new LoginPage()
                .EnterCredentials("xtc@xamarin.com", "fake")
                .TapLogin();

            page.RateThisLocker()
                .VerifyStarsIncrementally()
                .SubmitReview()
                .FeedbackReceived();
        }        

        [Test]
        public void RateLockerSignedIn()
        {
            SignIn();

            new LockersPage()
                .InvestigateFirstLocker();

            new LockerDetailsPage()
                .VerifyContentPresent()
                .RateThisLocker()
                .VerifyStarsIncrementally()
                .SubmitReview()
                .FeedbackReceived();
        }

        public void SignIn()
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
        }
    }
}

