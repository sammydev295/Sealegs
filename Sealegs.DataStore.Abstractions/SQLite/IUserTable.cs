using Sealegs.Utils;

namespace Sealegs.DataStore.Abstractions.SQLite
{
    public interface IUserTable
    {
        void InsertUser(User user);
        bool Update(User user);
        User GetUser();
        void DeleteAll();

    }
}
