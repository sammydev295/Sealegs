using System.Linq;
using System.Data.Entity;
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
    public class SealegsUserController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        [HttpGet]
        public SealegsUser Get(string userId)
        {

            return Context.SealegsUser.Where(item => item.UserID == new System.Guid(userId)).FirstOrDefault();
        }

        //[EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        //[HttpPost]
        //public bool Insert(Wine item)
        //{
        //    Context.Wine.Add(item);
        //    int result = Context.SaveChanges();
        //    return result < 0 ? false : true;
        //}

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Insert(SealegsUser user)
        {
            string salt = string.Empty;
            SimpleHash hash = new SimpleHash();
            user.Password = hash.Compute(user.Password, 5000, out salt);
            user.PasswordSalt = salt;
            user.Role = Context.SealegsUserRole.First(r => r.Id == SealegsUserRole.LockerMember.ToString());
            Context.SealegsUser.Add(user);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(SealegsUser user)
        {
            var original = Context.SealegsUser.Where(item => item.UserID == user.UserID).FirstOrDefault();
            if (original != null)
            {
                original.AvatarImage = user.AvatarImage;
                original.FirstName = user.FirstName;
                original.LastName = user.LastName;
                Context.Entry(original).CurrentValues.SetValues(original);
                int result = Context.SaveChanges();
                return result < 0 ? false : true;
            }
            return false;
        }
    }
}