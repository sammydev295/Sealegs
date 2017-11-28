using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sealegs.DataObjects;

namespace Sealegs.Clients.Portable
{
    public interface IFavoriteService
    {
        Task<bool> ToggleFavorite(LockerMember locker);
    }
}
