using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;
using Sealegs.DataStore.Azure;

namespace Sealegs.DataStore.Azure
{
    public class UserStore : BaseStore<SealegsUser>, IUserStore
    {
        public override string Identifier => "User";
    }
}

