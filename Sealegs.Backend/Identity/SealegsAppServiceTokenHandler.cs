using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using Microsoft.Azure.Mobile.Server.Authentication;

using Sealegs.DataObjects;
using Sealegs.Backend.Controllers;

namespace Sealegs.Backend.Identity
{
    public class SealegsAppServiceTokenHandler : AppServiceTokenHandler
    {
        #region CTOR

        public SealegsAppServiceTokenHandler(HttpConfiguration config)
            : base(config)
        {

        }

        #endregion 

        #region TryValidateLoginToken

        public override bool TryValidateLoginToken(
            string token,
            string signingKey,
            IEnumerable<string> validAudiences,
            IEnumerable<string> validIssuers,
            out ClaimsPrincipal claimsPrincipal)
        {
            var validated = base.TryValidateLoginToken(token, signingKey, validAudiences, validIssuers, out claimsPrincipal);
            if (validated)
            {
                // this is your custom role provider class which would lookup user roles by user id and inject these into the user claims identity
                var myRoleProvider = new SealegsRoleProvider();

                // get user id (sid)
                string sid = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;

                // get user roles (from database, for example)
                var role = myRoleProvider.GetUserRolesBySid(sid);

                // Add/Update the role claim
                //Claim claimRole = claimsPrincipal.FindFirst(ClaimTypes.Role);
                //if (claimRole != null && claimRole.Value != role.Id)
                //    ((ClaimsIdentity)claimsPrincipal.Identity).RemoveClaim(claimRole);

                //if (claimRole == null || claimRole.Value != role.Id)
                //    ((ClaimsIdentity)claimsPrincipal.Identity).AddClaim(new Claim(ClaimTypes.Role, role != null ? role.Id : SealegsUserRole.CustomerRole));
            }

            return validated;
        }
        
        #endregion 
    }

    #region SealegsRoleProvider

    internal class SealegsRoleProvider
    {
        #region CTOR

        public SealegsRoleProvider()
        {
        }

        #endregion 

        #region GetUserRolesBySid

        public SealegsUserRole GetUserRolesBySid(string sid)
        {
            // Todo - This list of roles for this user fetched from the database
            var user = new SealegsAuthClient().GetUser(sid);
            return user != null ? user.Role : null;
        }

        #endregion 
    }

    #endregion 
}
