using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using LinqToTwitter;
using Microsoft.Azure.Mobile.Server.Authentication;
using Sealegs.Backend.Controllers;
using Sealegs.Backend.Models;
using Sealegs.DataObjects;

namespace Sealegs.Backend.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class EmployeeAuthorizeAttribute : AuthorizeAttribute
    {
        #region CTOR

        public EmployeeAuthorizeAttribute() : base()
        {
            Roles = SealegsUserRole.CustomerRole;
        }

        public EmployeeAuthorizeAttribute(params String[] roles) : base()
        {
            Roles = String.Join(",", roles);
        }

        #endregion 

        #region OnAuthorization

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // If not already authenticated, return.
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var claims = identity.Claims;

            #region Email

            const string claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
            var emailClaim = (from c in claims
                         where c.Type == claimType
                         select c.Value);
            var email = emailClaim.Count() > 0 ? emailClaim.Single() : null;

            // If they don't have an identity name at all, return.
            if (string.IsNullOrEmpty(email))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            // If their name is not a valid email, return.
            var address = IsValidEmail(email);
            if (address == null)
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            #endregion

            #region Roles
         
            var userRole = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).Single();
            if (!Roles.Split(',').Any(c => c.ToLower() == userRole.ToLower()))
            {
                HandleUnauthorizedRequest(actionContext);
                return;
            }

            #endregion

            base.OnAuthorization(actionContext);

            // If user email does not contain xamarin.com, return - not needed for sealegs
            //if (!address.Host.ToLower().Equals(ConfigurationManager.AppSettings["CompanyDomain"]))
            //    throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        #endregion 

        protected override bool IsAuthorized(HttpActionContext actionContext) => true;

        #region IsValidEmail

        MailAddress IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr;
            }
            catch
            {
                return null;
            }
        }

        #endregion 
    }
}