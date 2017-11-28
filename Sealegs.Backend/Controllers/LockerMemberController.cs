using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.Tracing;

using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;
using System.Collections.Generic;

namespace Sealegs.Backend.Controllers
{
    public class LockerMemberController : ApiController
    {

        SealegsContext Context = new SealegsContext();

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        public IList<LockerMember> GetAll()
        {
            return Context.LockerMember.Where(item => item.Deleted == false).OrderBy(l => l.DisplayName).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        public IList<LockerMember> GetAllNonStaff()
        {
            return Context.LockerMember.Where(item => item.Deleted == false && item.IsStaff ==false).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        public IList<LockerMember> GetAllStaff()
        {
            return Context.LockerMember.Where(item => item.Deleted == false && item.IsStaff == true).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        public LockerMember Get(string id)
        {
            return Context.LockerMember.Where(item => item.Id == id).FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        public LockerMember GetByMemberId(string id)
        {
            return Context.LockerMember.Where(item => item.LockerMemberID == id).FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpPost]
        public bool Update(LockerMember item)
        {
            LockerMember original = Context.LockerMember.Find(item.Id);
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpPost]
        public bool Insert(LockerMember item)
        {
            Context.LockerMember.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            LockerMember original = Context.LockerMember.Find(id);

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