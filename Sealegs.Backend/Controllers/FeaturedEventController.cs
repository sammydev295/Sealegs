using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using Sealegs.Backend.Models;
using Sealegs.DataObjects;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class FeaturedEventController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        [AllowAnonymous]
        [HttpGet]
        public IList<FeaturedEvent> GetAll()
        {
            return Context.FeaturedEvent.Where(item => item.Deleted == false).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public FeaturedEvent Get(string id)
        {
            return Context.FeaturedEvent.Where(item => item.Id.Replace("\r\n", string.Empty).ToLower() == id.ToLower()).ToList().FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(FeaturedEvent item)
        {
            item.UpdatedAt = DateTime.Now;
            FeaturedEvent original = Context.FeaturedEvent.Find(item.Id);

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
        public bool Insert(FeaturedEvent item)
        {
            item.CreatedAt = item.UpdatedAt = DateTime.Now;
            Context.FeaturedEvent.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            FeaturedEvent original = Context.FeaturedEvent.Find(id);

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
