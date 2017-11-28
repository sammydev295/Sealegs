using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PCLStorage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sealegs.Clients.Portable
{
    public class AzureBlobStorage : IAzureBlobStorage
    {
        public CloudBlobContainer GetContainer(string containerName)
        {
            var account = CloudStorageAccount.Parse(Addresses.SealegsAzureStorageConnectionString);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(containerName.ToLower());
        }

        public async Task<Tuple<string, bool>> UploadTextFileAsync(string containerName, Stream stream)
        {
            var uploadedFilename = await UploadFileAsync(containerName, null, stream);
            return uploadedFilename;
        }

        public async Task<Tuple<string, bool>> UploadTextFileAsync(string containerName, String fullFileName)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(fullFileName);
            var text = await file.ReadAllTextAsync();
            var byteData = Encoding.UTF8.GetBytes(text);
            var uploadedFilename = await UploadFileAsync(containerName, null, new MemoryStream(byteData));

            return uploadedFilename;
        }

        public async Task<Tuple<string, bool>> UploadBinFileAsync(string containerName, Stream stream)
        {
            var uploadedFilename = await UploadFileAsync(containerName, null, stream);
            return uploadedFilename;
        }

        public async Task<Tuple<string, bool>> UploadBinFileAsync(string containerName, String fullFileName)
        {
            var file = await FileSystem.Current.GetFileFromPathAsync(fullFileName);
            var stream = await file.OpenAsync(FileAccess.Read);

            var uploadedFilename = await UploadFileAsync(containerName, null, stream);
            return uploadedFilename;
        }

        public async Task<Tuple<string, bool>> UploadFileAsync(string containerName, string storageLocation, Stream stream)
        {
            try
            {
                var container = GetContainer(containerName);
                //await container.CreateIfNotExistsAsync();
                var directory = container.GetDirectoryReference(storageLocation);
                var name = Guid.NewGuid()+".jpg";
                var blob = directory.GetBlockBlobReference(name);

                await Task.Run(() => blob.UploadFromStream(stream));

                return new Tuple<string, bool>(name, true);
            }
            catch (Exception ex)
            {
                return new Tuple<string, bool>(ex.Message, false); 
            }
        }

        public async Task<Tuple<string, bool>> UploadFileAsync(string containerName, string storageLocation, byte[] stream)
        {
            try
            {
                var container = GetContainer(containerName);
                //await container.CreateIfNotExistsAsync();
                var directory = container.GetDirectoryReference(storageLocation);
                var name = Guid.NewGuid()+".jpg";
                var blob = directory.GetBlockBlobReference(name);

                // await blob.UploadFromStreamAsync(stream);
                blob.UploadFromByteArray(stream,0,stream.Length);
                //  await Task.Run(() => blob.UploadFromStream(stream));

                return new Tuple<string, bool>(name, true);
            }
            catch (Exception ex)
            {
                return new Tuple<string, bool>(ex.Message, false);
            }

        }

        public async Task<string> DownloadTextFileAsync(string containerName, string fileName)
        {
            var byteData = await DownloadtFileAsync(containerName, fileName);
            string text = Encoding.UTF8.GetString(byteData, 0, byteData.Length);

            return text;
        }

        public async Task<byte[]> DownloadBinFileAsync(string containerName, string fileName)
        {
            var byteData = await DownloadtFileAsync(containerName, fileName);

            return byteData;
        }

        public async Task<byte[]> DownloadtFileAsync(string containerName, string name)
        {
            var container = GetContainer(containerName);

            var blob = container.GetBlobReference(name);
            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];

                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public async Task<IList<string>> GetFilesListAsync(string containerName)
        {
            var container = GetContainer(containerName);

            var allBlobsList = new List<string>();
            BlobContinuationToken token = null;

            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    allBlobsList.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);

            return allBlobsList;
        }

        public async Task<bool> ExistsAsync(string containerName, string blobName)
        {
            var container = GetContainer(containerName);
            return await container.GetBlockBlobReference(blobName).ExistsAsync();
        }

        public async Task<bool> DeleteFileAsync(string containerName, string name)
        {
            var container = GetContainer(containerName);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }
    }
}
