using System;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IFeedbackStore : IBaseStore<Feedback>
    {
        Task<bool> LeftFeedback(LockerMember locker);

        Task DropFeedback();
    }
}

