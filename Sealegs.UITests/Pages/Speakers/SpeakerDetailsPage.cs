using System;
using NUnit.Framework;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace Sealegs.UITests
{
    public class SpeakerDetailsPage : BasePage
    {
        readonly string MoreLockers = "MoreLockersSection";

        public SpeakerDetailsPage()
            : base ("Speaker Info", "Speaker Info")
        {
        }

        public SpeakerDetailsPage ValidateAdditionalLockers(bool yesorno)
        {
            try
            {
                app.ScrollDownTo(MoreLockers, timeout:TimeSpan.FromSeconds(5));
                if (!yesorno)
                    Assert.IsTrue(false, message: "Expected no additional lockers, but found more");
            }
            catch
            {
                app.Screenshot("No Additional Lockers Found");
                if (yesorno)
                    Assert.IsTrue(false, message: "Expected additional lockers, but found none");
            }

            app.Screenshot("Locker Numbers Verified");

            return this;
        }
    }
}

