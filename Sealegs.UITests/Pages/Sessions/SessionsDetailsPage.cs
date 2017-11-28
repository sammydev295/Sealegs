using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using System.Threading;

namespace Sealegs.UITests
{
    public class LockerDetailsPage : BasePage
    {
        readonly string LockerTitle = "LockerTitle";
        readonly string LockerDate = "LockerDate";
        readonly string LockerAbstract = "LockerAbstract";
        readonly string LockerSpeakers = "LockerSpeakers";
        readonly string RateButton = "Rate this Locker";
        readonly string SubmitButton = "Submit";
        readonly string FeedbackMessage = "Thanks for the feedback, have a great Evolve.";
        readonly Query FirstSpeaker;
        readonly string StarImage;

        public LockerDetailsPage()
            : base("Locker Details", "Locker Details")
        {
            if (OnAndroid)
            {
                FirstSpeaker = x => x.Marked("LockerSpeakers").Descendant().Class("FormsTextView").Index(0);
                StarImage = "FormsImageView";
            }
            if (OniOS)
            {
                FirstSpeaker = x => x.Id("LockerSpeakers").Descendant().Class("Xamarin_Forms_Platform_iOS_ViewCellRenderer_ViewTableCell").Index(0);
                StarImage = "UIImageView";
            }
        }

        public Query StarNumber(int number)
        {
            int newnumber = OniOS ? (number - 1) * 2 : number * 2;
            return x => x.Class(StarImage).Index(newnumber);
        }

        public LockerDetailsPage VerifyContentPresent()
        {
            Assert.IsNotNull(app.Query(LockerTitle)[0].Text, "Locker Title Not Found");
            Assert.IsNotNull(app.Query(LockerDate)[0].Text, "Locker Date Not Found");
            app.ScrollDownTo(LockerAbstract);
            Assert.IsNotNull(app.Query(LockerAbstract)[0].Text, "Locker Abstract Not Found");
            app.ScrollDownTo(LockerSpeakers);
            Assert.IsNotNull(app.Query(LockerSpeakers)[0], "Locker Speakers Not Found");
            app.Screenshot("Locker information verified as present");

            return this;
        }

        public void GoToSpeakerDetails()
        {
            app.ScrollDownTo("FeedbackTitle", timeout:TimeSpan.FromSeconds(10));
            app.Screenshot("Scrolled Down to Locker Speakers");
            app.Tap(FirstSpeaker);
            app.Screenshot("Selected first speaker");
        }

        public LockerDetailsPage RateThisLocker()
        {
            app.ScrollDown();
            app.Tap(RateButton);
            app.Screenshot("Tapped: 'Rate This Locker'");
            return this;
        }

        public LockerDetailsPage VerifyStarsIncrementally()
        {
            app.Tap(StarNumber(1)); 
            app.WaitForElement(x => x.Text("Not a fan"));
            app.Screenshot("1 star message");
            app.Tap(StarNumber(2));
            app.WaitForElement(x => x.Text("It was ok"));
            app.Screenshot("2 star message");
            app.Tap(StarNumber(3));
            app.WaitForElement(x => x.Text("Good"));
            app.Screenshot("3 star message");
            app.Tap(StarNumber(4));
            app.WaitForElement(x => x.Text("Great"));
            app.Screenshot("4 star message");
            app.Tap(StarNumber(5));
            app.WaitForElement(x => x.Text("Love it!"));
            app.Screenshot("5 star message");

            return this;
        }

        public LockerDetailsPage SubmitReview()
        {
            app.Tap(SubmitButton);
            app.WaitForElement(FeedbackMessage);
            app.Screenshot("Feedback dialog appears");

            app.Tap("OK");

            return this;
        }

        public void FeedbackReceived()
        {
            app.WaitForElement("Thanks for your feedback!");
            app.Screenshot("Feedback received");
        }
    }
}

