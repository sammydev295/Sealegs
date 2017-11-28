using System;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

using NUnit.Framework;

namespace Sealegs.UITests
{
    public class EventDetailsPage : BasePage
    {
        readonly string EventTitle = "EventTitle";
        readonly string EventTime = "EventTime";
        readonly string EventDescription = "EventDescription";
        readonly string LoacationName = "EventLocationRoom";
        readonly string ReminderButton;
        readonly string NewsName = "NewsName";
        readonly string NewsImage = "NewsImage";
        readonly string NewsLevel = "NewsLevel";
        readonly string NewsCell = "NewsCell";

        public EventDetailsPage()
            : base(x => x.Class("Toolbar").Descendant().Text("Event Details"), x => x.Class("UINavigationBar").Id("Event Details"))
        {
            if (OnAndroid)
            {
                ReminderButton = "AndroidReminderButton";
            }
            if (OniOS)
            {
                ReminderButton = "iOSReminderButton";
            }
        }

        public EventDetailsPage VerifyContentPresent()
        {
            app.WaitForElement(EventTitle);

            Assert.IsNotNull(app.Query(EventTitle).First().Text);
            Assert.IsNotNull(app.Query(EventTime).First().Text);
            Assert.IsNotNull(app.Query(EventDescription).First().Text);
            Assert.IsNotNull(app.Query(LoacationName).First().Text);

            return this;
        }

        public EventDetailsPage VerifyNewsPresent()
        {
            Assert.IsNotNull(app.Query(NewsName).First().Text);
            Assert.IsNotNull(app.Query(NewsLevel).First().Text);
            app.WaitForElement(NewsImage);
            Assert.IsNotNull(app.Query(NewsImage).First().Description);

            return this;
        }

        public void SelectNews()
        {
            app.Screenshot("Selecting News");
            app.WaitForElement(NewsCell);
            app.Tap(NewsCell);
        }

    }
}

