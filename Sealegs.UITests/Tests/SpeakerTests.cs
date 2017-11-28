using System;
using Xamarin.UITest;
using NUnit.Framework;

namespace Sealegs.UITests
{
    public class SpeakerTests : AbstractSetup
    {
        public SpeakerTests(Platform platform)
            : base(platform)
        {
        }

        [Test]
        public void MultipleLockers()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .InvestigateFirstLocker();

            new LockerDetailsPage()
                .GoToSpeakerDetails();

            new SpeakerDetailsPage()
                .ValidateAdditionalLockers(true);
        }

        [Test]
        public void SingleLocker()
        {
            new FeedPage().NavigateTo("Lockers");

            new LockersPage()
                .InvestigateLockerMarked("Everyone can create beautiful apps with material design");

            new LockerDetailsPage()
                .GoToSpeakerDetails();

            new SpeakerDetailsPage()
                .ValidateAdditionalLockers(false);
        }

    }
}

