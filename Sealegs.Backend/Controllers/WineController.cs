using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.Tracing;
using System.Data.Entity;

using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;
using Sealegs.Backend.Helpers;

namespace Sealegs.Backend.Controllers
{
    [MobileAppController]
    public class WineController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<Wine> GetAll(string lockerId)
        {
           return Context.Wine.Where(item => item.LockerID == lockerId &&  item.Deleted == false).Include("WineVarietal").OrderByDescending(w => w.CheckedInDate).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public Wine Get(string id)
        {
            return Context.Wine.Where(item => item.Id == id).FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(Wine item)
        {
            Wine original = Context.Wine.Find(item.Id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Insert(Wine item)
        {
            Context.Wine.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            Wine original = Context.Wine.Find(id);

            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(original.Deleted = true);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }
    }
}