using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Sealegs.Backend.Models;
using Sealegs.DataObjects;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class MiniHackController : ApiController
    {



        //public IQueryable<MiniHack> GetAllMiniHack()
        //{
        //    return Query(); 
        //}

        //public SingleResult<MiniHack> GetMiniHack(string id)
        //{
        //    return Lookup(id);
        //}

        //[EmployeeAuthorize]
        //public Task<MiniHack> PatchMiniHack(string id, Delta<MiniHack> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}

        //[EmployeeAuthorize]
        //public async Task<IHttpActionResult> PostMiniHack(MiniHack item)
        //{
        //    MiniHack current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //[EmployeeAuthorize]
        //public Task DeleteMiniHack(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}
