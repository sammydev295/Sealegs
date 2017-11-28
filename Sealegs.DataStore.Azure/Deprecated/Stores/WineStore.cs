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
using MoreLinq;

using Sealegs.DataObjects;
using Sealegs.DataStore.Abstractions;

namespace Sealegs.DataStore.Azure
{
    public class WineStore : BaseStore<Wine>, IWineStore
    {
        public override string Identifier => "WinesA";

        public async Task<IEnumerable<Wine>> GetAllByLocker(string lockerId, bool forceRefresh = false )
        {
            try
            {
                await InitializeStore().ConfigureAwait(false);
                if (forceRefresh)
                    await PullLatestAsync(lockerId).ConfigureAwait(false);

                bool ret = await SyncAsync(lockerId);
                var parameters = new Dictionary<string, string>();
                parameters.Add("lockerId", lockerId);
                var wines1 = await Table.ReadAsync();

                var wines = await Table.ToListAsync().ConfigureAwait(false);

                
                foreach(var item in wines1)
                {
                    var a = item.WineTitle;
                    var b = item.Vintage;
                }
                // Adjust image paths
                wines.ForEach(w =>
                {
                    var img = w?.ImagePath ?? Wine.DefaultBottleImage;
                    w.ImagePath = $"{base.ImagesURI}/{img}";
                    img = w?.CheckedOutEmployeeSignature ?? String.Empty;
                    w.CheckedOutEmployeeSignature = $"{base.ImagesURI}/{img}";
                    img = w?.CheckedOutMemberSignature ?? String.Empty;
                    w.CheckedOutMemberSignature = $"{base.ImagesURI}/{img}";
                });

                return wines;
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        public async Task<Wine> GetSingle(int id)
        {
            return null;
        }

        public async Task<bool> SyncAsync(string lockerId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to sync items, we are offline");
                return false;
            }

            try
            {
                
                await StoreManager.MobileService.SyncContext.PushAsync();
                if (!(await PullLatestAsync(lockerId)))
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync items, that is alright as we have offline capabilities: " + ex);
                return false;
            }
            finally
            {
            }
            return true;
        }

        public async Task<bool> PullLatestAsync(string lockerId)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Debug.WriteLine("Unable to pull items, we are offline");
                return false;
            }

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("lockerId", lockerId);

                var query = Table.CreateQuery().WithParameters(parameters);
                var ret0 = Table.PurgeAsync($"{Identifier}", query, new CancellationToken());
                var options = new PullOptions() { MaxPageSize = 4000 };
                var ret = Table.PullAsync($"{Identifier}", query.IncludeTotalCount(), true, new CancellationToken(), options);

                var totalCount = query.RequestTotalCount;
                var itemsList = await query.ToListAsync().ConfigureAwait(false);
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

    }
}
