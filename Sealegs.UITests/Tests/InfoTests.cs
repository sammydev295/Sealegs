using System;

using Xamarin.UITest;

using NUnit.Framework;

namespace Sealegs.UITests
{
    public class InfoTests : AbstractSetup
    {
        public InfoTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void NavigateToNews()
        {
            if (OnAndroid)
            {
                new FeedPage()
                    .NavigateTo("News");
            }

            if (OniOS)
            {
                new FeedPage()
                    .NavigateTo("Info");

                new InfoPage()
                    .NavigateToInfoItem("News");
            }

            new NewsPage()
                .VerifyContentPresent()
                .SelectNews("Dropbox");

            new NewsDetailsPage()
                .VerifyContentPresent();
        }

        [Test]
        public void WifiSetup()
        {
            if (OnAndroid)
            {
                new FeedPage()
                    .NavigateTo("Conference Info");

                new WiFiInformationPage()
                    .SetUpWifi()
                    .CopyPasswords();
            }

            if (OniOS)
            {
                new FeedPage()
                    .NavigateTo("Info");

                new InfoPage()
                    .NavigateToInfoItem("Wi-Fi Information");

                new WiFiInformationPage()
                    .CopyPasswords();
            }
        }
    }
}

