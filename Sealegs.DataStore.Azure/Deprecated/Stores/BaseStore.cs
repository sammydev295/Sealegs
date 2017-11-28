using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
using Plugin.Connectivity;

using Sealegs.DataStore.Abstractions;
using Sealegs.DataObjects;

namespace Sealegs.DataStore.Azure
{
    public class BaseStore<T> : IBaseStore<T> where T : class, IBaseDataObject, new()
    {
        #region Fields

        IStoreManager storeManager;
        public string ImagesURI { get; set; }

        #endregion

        #region Properties
        public virtual string Identifier => "Items";

        IMobileServiceSyncTable<T> table;
        protected IMobileServiceSyncTable<T> Table
        {
            get { return table ?? (table = StoreManager.MobileService.GetSyncTable<T>()); }
          
        }

        #endregion

        #region CTOR

        public BaseStore()
        {
            
        }

        #endregion

        #region IBaseStore implementation

        public async Task InitializeStore()
        {
            if (storeManager == null)
                storeManager = DependencyService.Get<IStoreManager>();

            if (!storeManager.IsInitialized)
                await storeManager.InitializeAsync();
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh)
        {
            try
            {
                await InitializeStore();
                if (forceRefresh)
                    await PullLatestAsync();

                var items = await Table.ToEnumerableAsync();
                return items;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            await InitializeStore();
            await PullLatestAsync();

            var items = await Table.Where(s => s.Id == id).ToListAsync();
            if (items == null || items.Count == 0)
                return null;

            return items[0];
        }

        public virtual async Task<bool> InsertAsync(T item)
        {
            await InitializeStore();
            await PullLatestAsync();
            await Table.InsertAsync(item);
            await SyncAsync();
            return true;
        }

        public virtual async Task<bool> UpdateAsync(T item)
        {
            await InitializeStore();
            await Table.UpdateAsync(item);
            await SyncAsync();

            return true;
        }

        public virtual async Task<bool> RemoveAsync(T item)
        {
            await InitializeStore();
            await PullLatestAsync ();
            await Table.DeleteAsync(item);
            await SyncAsync();

            return true;
        }

        public virtual async Task<bool> PullLatestAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to pull items, we are offline");
                return false;
            }

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                var query = Table.CreateQuery();
                query.IncludeTotalCount();
                await Table.PullAsync($"{Identifier}", query, true, new CancellationToken(), new PullOptions());

                var ItemsList = await query.ToListAsync();
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to pull items, that is alright as we have offline capabilities: " + ex);
            }
            
            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }

                return false;
            }

            return true;
        }

        public async Task<bool> SyncAsync()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to sync items, we are offline");
                return false;
            }

            try
            {

                await StoreManager.MobileService.SyncContext.PushAsync();
                if(!(await PullLatestAsync()))
                    return false;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Unable to sync items, that is alright as we have offline capabilities: " + ex);
                return false;
            }
            finally
            {
            }
            return true;
        }

        public void DropTable()
        {
            table = null;
        }

        #endregion
    }
}

