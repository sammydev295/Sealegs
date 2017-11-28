using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

using Microsoft.Azure.Mobile.Server;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class MemberSuggestionController : ApiController
    {



        //public IQueryable<MemberSuggestion> GetAll()
        //{
        //    return Query();
        //}

        //public SingleResult<MemberSuggestion> Get(string id)
        //{
        //    return Lookup(id);
        //}

        //public IQueryable<MemberSuggestion> GetByLockerMember(string id)
        //{
        //    return Query().Where(l => l.LockerMemberID == new Guid(id));
        //}

        //[EmployeeAuthorize]
        //public Task<MemberSuggestion> Patch(string id, Delta<MemberSuggestion> patch)
        //{
        //    return UpdateAsync(id, patch);
        //}

        //[EmployeeAuthorize]
        //public async Task<IHttpActionResult> Post(MemberSuggestion item)
        //{
        //    MemberSuggestion current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //[EmployeeAuthorize]
        //public Task Delete(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}