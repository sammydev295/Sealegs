using System.Linq;
using System.Web.Http;
using System.Collections.Generic;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class FavoriteController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole)]
        [HttpGet]
        public IList<Favorite> GetAll()
        {
            return Context.Favorites.Where(item => !item.Deleted).ToList();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpGet]
        public IList<Favorite> GetAll(string Id)
        {
            return Context.Favorites.Where(item => item.Id == Id && !item.Deleted).ToList();
        }

        //[EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.CustomerRole, SealegsUserRole.LockerMemberFriendRole)]
        //[HttpGet]
        //public Wine Get(string id)
        //{
        //    return Context.Wine.Where(item => item.Id == id).FirstOrDefault();
        //}

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole, SealegsUserRole.LockerMemberRole, SealegsUserRole.LockerMemberFriendRole)]
        [HttpPost]
        public bool Update(Favorite item)
        {
            Favorite original = Context.Favorites.Find(item.Id);
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(item);
                int result = Context.SaveChanges();
                return result >= 0;
            }

            return false;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Insert(Favorite item)
        {
            Context.Favorites.Add(item);
            int result = Context.SaveChanges();
            return result >= 0;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            Favorite original = Context.Favorites.Find(id);
            if (original != null)
            {
                Context.Entry(original).CurrentValues.SetValues(original.Deleted = true);
                int result = Context.SaveChanges();
                return result >= 0;
            }

            return false;
        }

    }
}