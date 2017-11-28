using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using System.Web.Http.Tracing;

using Microsoft.Azure.Mobile.Server;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;
using System.Collections.Generic;
using System.Globalization;

namespace Sealegs.Backend.Controllers
{
    public class NewsController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        [AllowAnonymous]
        [HttpGet]
        public IList<News> GetAll()
        {
            return Context.News.Where(news => news.Deleted == false).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public News Get(string id)
        {
            return Context.News.Where(news => news.Id.Replace("\r\n", string.Empty).ToLower() == id.ToLower()).ToList().FirstOrDefault();
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpPost]
        public bool Update(News item)
        {
            item.UpdatedAt = DateTime.Now;
            News original = Context.News.Find(item.Id);

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
        public bool Insert(News item)
        {
            DateTimeOffset date = DateTime.Now;
            
            item.CreatedAt = item.UpdatedAt = DateTime.Now; 
            Context.News.Add(item);
            int result = Context.SaveChanges();
            return result < 0 ? false : true;
        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole, SealegsUserRole.EmployeeRole)]
        [HttpGet]
        public bool Delete(string id)
        {
            News original = Context.News.Find(id);
            
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