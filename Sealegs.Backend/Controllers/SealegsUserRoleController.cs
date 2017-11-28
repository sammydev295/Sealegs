using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

using Microsoft.Azure.Mobile.Server;

using Sealegs.DataObjects;
using Sealegs.Backend.Models;
using Sealegs.Backend.Identity;
using System.Collections.Generic;

namespace Sealegs.Backend.Controllers
{
    public class SealegsUserRoleController : ApiController
    {
        SealegsContext Context = new SealegsContext();

        // GET api/Default
        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole )]
        [HttpGet]
        public IList<SealegsUserRole> GetAll()
        {

            return Context.SealegsUserRole.Where(item => item.Deleted == false).ToList();

        }

        [EmployeeAuthorize(SealegsUserRole.AdminRole, SealegsUserRole.ManagerRole, SealegsUserRole.ServerRole)]
        [HttpGet]
        public SealegsUserRole Get(string id)
        {
            return Context.SealegsUserRole.Where(item => item.Id == id).FirstOrDefault();
        }
    }
}