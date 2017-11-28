using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Sealegs.DataObjects;

using Sealegs.DataStore.Azure;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure
{
    public class FeedbackStore : BaseStore<Feedback>, IFeedbackStore
    {
        public async Task<bool> LeftFeedback(LockerMember locker)
        {
            await InitializeStore();
            var items = await Table.Where(s => s.FeedbackEntityId == locker.Id).ToListAsync().ConfigureAwait (false);
            return items.Count > 0;
        }

        public Task DropFeedback()
        {
            return Task.FromResult(true);
        }

        public override string Identifier => "Feedback";
    }
}

