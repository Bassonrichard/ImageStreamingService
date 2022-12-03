using Azure.Storage.Blobs;
using ImageStreamService.Configuration;
using Microsoft.Extensions.Options;

namespace ImageStreamService.Services
{
    public interface IStorageService
    {
        Task UploadBlob(Stream file, string blobName, string containerName);
    }

    public class StorageService : IStorageService
    {
        public StoragAccountConfiguration _storageAccountConfig { get; set; }

        public StorageService(IOptions<StoragAccountConfiguration> storageAccountConfig)
        {
            _storageAccountConfig = storageAccountConfig.Value;
        }

        public async Task UploadBlob(Stream file, string blobName, string containerName)
        {
            try
            {
                var container = new BlobContainerClient(_storageAccountConfig.ConnectionString, containerName.ToLower());
                await container.CreateIfNotExistsAsync();

                BlobClient blob = container.GetBlobClient(blobName);

                await blob.UploadAsync(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
