using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Sealegs.DataObjects;

namespace Sealegs.DataStore.Abstractions
{


    public interface IBaseStore<T>
    {
        Task InitializeStore();
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<T> GetItemAsync(string id);
        Task<bool> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> RemoveAsync(T item);
        Task<bool> SyncAsync();

        void DropTable();

        string Identifier { get; }

        string ImagesURI { get; set; }
    }

    //public interface IBaseStore<T>
    //{
    //    Task InitializeStore();
    //    Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    //    Task<T> GetItemAsync(string id);
    //    Task<bool> InsertAsync(T item);
    //    Task<bool> UpdateAsync(T item);
    //    Task<bool> RemoveAsync(T item);
    //    Task<bool> SyncAsync();

    //    void DropTable();

    //    string Identifier { get; }

    //    string ImagesURI { get; set; }
    //}
}

