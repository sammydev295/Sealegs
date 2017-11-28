using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

using Microsoft.Azure.Mobile.Server.Login;

using Newtonsoft.Json.Linq;

using Sealegs.DataObjects;
using Sealegs.Backend.Identity;

namespace Sealegs.Backend.Controllers
{
    public class SealegsAuthController : ApiController
    {
        #region Constants

        public const string SealegsAuthApiKeyName = "SigningKey";
        public const string AuthSigningKeyVariableName = "WEBSITE_AUTH_SIGNING_KEY";
        public const string HostNameVariableName = "WEBSITE_HOSTNAME";

        #endregion 

        #region Post

        public async Task<IHttpActionResult> Post([FromBody] JObject assertion)
        {
            SealegsAuthResponse authResult = AuthenticateCredentials(assertion.ToObject<SealegsCredentials>());
            if (!authResult.Success)
                return Unauthorized();

            IEnumerable<Claim> claims = GetAccountClaims(authResult.User);
            string websiteUri = $"https://{WebsiteHostName}/";

            JwtSecurityToken token = AppServiceLoginHandler.CreateToken(claims, TokenSigningKey, websiteUri, websiteUri, TimeSpan.FromDays(10));

            return await Task.FromResult(Ok(new LoginResult { RawToken = token.RawData, User = authResult.User }));
        }

        #endregion 

        #region GetAccountClaims

        private IEnumerable<Claim> GetAccountClaims(SealegsUser user) => new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

        #endregion 

        #region AuthenticateCredentials

        private SealegsAuthResponse AuthenticateCredentials(SealegsCredentials credentials)
        {
            string authApiKey = ConfigurationManager.AppSettings[SealegsAuthApiKeyName];
            if (authApiKey == null)
                throw new InvalidOperationException("Missing SealegsAuthApiKey configuration setting.");

            using (var authClient = new SealegsAuthClient())
            {
                return authClient.GetAuthenticationToken(credentials);
            }
        }

        #endregion 

        #region TokenSigningKey

        private string TokenSigningKey => Environment.GetEnvironmentVariable(AuthSigningKeyVariableName) ?? "test_key";

        #endregion 

        #region WebsiteHostName

        public string WebsiteHostName => Environment.GetEnvironmentVariable(HostNameVariableName) ?? "localhost";

        #endregion 
    }
}
