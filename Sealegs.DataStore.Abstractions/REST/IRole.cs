using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions.REST
{
    public interface IRole
    {
        Task<IEnumerable<DataObjects.SealegsUserRole>> GetAllRoles();
    }
}
