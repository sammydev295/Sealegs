using System;
using System.Threading.Tasks;
using Sealegs.Utils;

namespace Sealegs.Clients.Portable
{
    public interface ISSOClient
    {
        Task<AccountResponse> LoginAsync(string username, string password);

        Task LogoutAsync();
    }
}

