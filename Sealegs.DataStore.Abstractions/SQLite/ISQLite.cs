using SQLite;

namespace Sealegs.DataStore.Abstractions.SQLite
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
