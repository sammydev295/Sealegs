using System;
using System.Collections.Generic;
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
    public class LockerTypeController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<LockerType> GetAll()
        {
            var lt = Context.LockerType.Where(item => item.Deleted == false).ToList();
            var cnt = lt.Count();
            return Context.LockerType.Where(item => item.Deleted == false).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public LockerType Get(string id)
        {
            return Context.LockerType.Where(item => item.Id == id).FirstOrDefault();
        }
    }
}