using System;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IEventStore : IBaseStore<FeaturedEvent>
    {
    }
}

