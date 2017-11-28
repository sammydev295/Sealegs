using System;

using NUnit.Framework;

using Xamarin.UITest;

namespace Sealegs.UITests
{
    public class EventsTests : AbstractSetup
    {
        public EventsTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void NavigateToEvent()
        {
            new FeedPage()
                .NavigateTo("Events");

            new EventsPage()
                .VerifyContentPresent()
                .SelectEvent("Darwin Lounge");

            new EventDetailsPage()
                .VerifyContentPresent();
        }

        [Test]
        public void HappyHourNewsCheck()
        {
            new FeedPage()
                .NavigateTo("Events");

            new EventsPage()
                .SelectEvent("Happy Hour");

            new EventDetailsPage()
                .VerifyNewsPresent()
                .SelectNews();

            new NewsDetailsPage()
                .VerifyContentPresent();

        }
    }
}

