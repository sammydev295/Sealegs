using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

using Sealegs.Clients.Portable;
using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure.Api
{
    public class Auth : ApiBase, IAuth
    {
        #region Implementations

        public async Task<MobileServiceUser> LoginAsync(string username, string password)
        {
            MobileService = new MobileServiceClient(Constants.APIBase);
            var credentials = new JObject
            {
                ["email"] = username,
                ["password"] = password
            };
            try
            {
                var mobileServiceUser = await MobileService.LoginAsync("SealegsAuth", credentials);
                Addresses.Token = mobileServiceUser.MobileServiceAuthenticationToken;
                return mobileServiceUser;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to login: " + ex);
                return null;
            }
        }

        public async Task<bool> Insert(SealegsUser sealegsUser)
        {
            var form = new Dictionary<string, string>
            {
                {"Id", sealegsUser.Id },
                { "UserID", sealegsUser.UserID.ToString() },
                { "Email", sealegsUser.Email },
                { "FirstName", sealegsUser.FirstName },
                { "LastName", sealegsUser.LastName },
                { "IsAnnonymous", sealegsUser.IsAnnonymous.ToString() },
                { "IsApproved", sealegsUser.IsApproved.ToString() },
                { "Password", sealegsUser.Password },
                { "PasswordFormat", "2" },
            };

            var result = await ClientBase.HttpPostRequest<bool>(InsertUserApi, form);
            return result;
        }

        public async Task<bool> Update(SealegsUser sealegsUser)
        {
            var form = new Dictionary<string, string>
            {
                {"Id", sealegsUser.Id },  {"FirstName", sealegsUser.FirstName }, {"AvatarImage", sealegsUser.AvatarImage },
            };

            var result = await ClientBase.HttpPostRequest<bool>(UpdateUserImageUri, form);
            return result;
        }

        public async Task<SealegsUser> GetUser(string userId)
        {
            var api = String.Format(GetUserUri, userId);
            var result = await ClientBase.HttpGetRequest<SealegsUser>(api);
            result.AvatarImage = (result.AvatarImage != null) ? String.Concat(Addresses.LockersStorageBaseAddress, result.AvatarImage) : String.Concat(Addresses.LockersStorageBaseAddress, DataObjects.LockerMember.DefaultProfileImage);

            return result;
        }
        #endregion

    }
}
