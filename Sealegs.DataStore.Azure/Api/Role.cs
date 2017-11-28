using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions.REST;

namespace Sealegs.DataStore.Azure.Api
{
    public class Role : ApiBase, IRole
    {
        public async Task<IEnumerable<DataObjects.SealegsUserRole>> GetAllRoles()
        {
            var result = await ClientBase.HttpGetRequest<IEnumerable<DataObjects.SealegsUserRole>>(GetAllRolesUri);
            return result;
        }
    }
}
