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
    public interface IAzureBlobStorage
    {
        CloudBlobContainer GetContainer(string containerType);

        Task<bool> ExistsAsync(string containerName, string blobName);

        Task<IList<string>> GetFilesListAsync(string containerName);

        Task<Tuple<string, bool>> UploadTextFileAsync(string containerName, Stream stream);
        Task<Tuple<string, bool>> UploadTextFileAsync(string containerName, String fullFileName);
        Task<Tuple<string, bool>> UploadBinFileAsync(string containerName, Stream stream);
        Task<Tuple<string, bool>> UploadBinFileAsync(string containerName, String fullFileName);
        Task<Tuple<string, bool>> UploadFileAsync(string containerName,string storageLocation, Stream stream);
        Task<Tuple<string, bool>> UploadFileAsync(string containerName, string storageLocation, byte[] stream);
        Task<string> DownloadTextFileAsync(string containerName, string fileName);
        Task<byte[]> DownloadBinFileAsync(string containerName, string fileName);
        Task<byte[]> DownloadtFileAsync(string containerName, string name);

        Task<bool> DeleteFileAsync(string containerName, string name);
    }
}
