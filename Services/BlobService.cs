using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureStorageApi.Services
{
       public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration config)

        {
            var connectionString = config["AzureStorage:Blob:ConnectionString"];
            var containerName = config["AzureStorage:Blob:ContainerName"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }


        public async Task<List<string>> ListBlobsAsync()
        {
            var blobs = new List<string>();
            await foreach (BlobItem blobItem in _containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return blobs;
        }

        public async Task UploadBlobAsync(string blobName, Stream content)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, overwrite: true);
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteBlobAsync(string blobName)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}
