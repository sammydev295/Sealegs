using System;
using System.Threading.Tasks;


using Sealegs.DataObjects;
using Sealegs.DataStore.Mock;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Mock
{
    public class FeedbackStore : BaseStore<Feedback>, IFeedbackStore
    {
        public Task<bool> LeftFeedback(LockerMember locker)
        {
            return Task.FromResult(Settings.LeftFeedback(locker.Id));
        }

        public async Task DropFeedback()
        {
            await Settings.ClearFeedback();
        }

        public override Task<bool> InsertAsync(Feedback item)
        {
            Settings.LeaveFeedback(item.FeedbackEntityId, true);
            return Task.FromResult(true);
        }

        public override Task<bool> RemoveAsync(Feedback item)
        {
            Settings.LeaveFeedback(item.FeedbackEntityId, false);
            return Task.FromResult(true);
        }
    }
}

