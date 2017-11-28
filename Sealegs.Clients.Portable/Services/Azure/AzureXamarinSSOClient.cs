using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sealegs.Utils;
using Sealegs.DataStore.Azure;
using Sealegs.DataStore.Abstractions;

using Xamarin.Forms;

using Microsoft.WindowsAzure.MobileServices;

namespace Sealegs.Clients.Portable.Auth.Azure
{
    public sealed class XamarinSSOClient : ISSOClient
    {
        private readonly StoreManager storeManager;
     
        public XamarinSSOClient(StoreManager storeManager)
        {
            if (storeManager == null)
            {
                throw new ArgumentNullException(nameof(storeManager));
            }

            this.storeManager = storeManager;
        }

        public XamarinSSOClient()
        {
            storeManager = DependencyService.Get<IStoreManager>() as StoreManager;

            if (storeManager == null)
            {
                throw new InvalidOperationException($"The {typeof(XamarinSSOClient).FullName} requires a {typeof(StoreManager).FullName}.");
            }
        }

        public async Task<AccountResponse> LoginAsync(string username, string password)
        {
            MobileServiceUser user = await storeManager.LoginAsync(username, password);
            return AccountFromMobileServiceUser(user);
        }

        public async Task LogoutAsync()
        {
            await storeManager.LogoutAsync();
        }

        private AccountResponse AccountFromMobileServiceUser(MobileServiceUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IDictionary<string, string> claims = JwtUtility.GetClaims(user.MobileServiceAuthenticationToken);

            var account = new AccountResponse();
            account.Success = true;
            account.User = new User
            {
                //Id = Guid.Parse(user.UserId),
                Id = Guid.Parse(claims[JwtClaimNames.Subject]),
                FirstName = claims[JwtClaimNames.GivenName],
                LastName = claims[JwtClaimNames.FamilyName],
                Email = claims[JwtClaimNames.Email],
                Role = claims[JwtClaimNames.Role],

            };
            account.Token = user.MobileServiceAuthenticationToken;
            
            return account;
        }
    }
}
