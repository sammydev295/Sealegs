using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{
    public interface IAuth
    {
        Task<MobileServiceUser> LoginAsync(string username, string password);

        Task<SealegsUser> GetUser(string userId);

        Task<bool> Insert(SealegsUser sealegsUser);

        Task<bool> Update(SealegsUser sealegsUser);
    }
}
