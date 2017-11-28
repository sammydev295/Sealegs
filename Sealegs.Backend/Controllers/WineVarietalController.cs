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
    public class WineVarietalController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<WineVarietal> GetAll()
        {

            return Context.WineVarietal.Where(item =>item.Deleted == false).ToList();

        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public WineVarietal Get(string id)
        {
            return Context.WineVarietal.Where(item => item.Id == id).FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(WineVarietal item)
        {
            WineVarietal original = Context.WineVarietal.Find(item.Id);

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
        public bool Insert(WineVarietal item)
        {
            Context.WineVarietal.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            WineVarietal original = Context.WineVarietal.Find(id);

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