using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace AzureStorageApi.Services
{
    public class FileShareService
    {
        private readonly ShareClient _shareClient;

        public FileShareService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:FileShare:ConnectionString"];
            var shareName = config["AzureStorage:FileShare:ShareName"];
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task UploadFileAsync(string fileName, Stream content)
        {
            var rootDir = _shareClient.GetRootDirectoryClient();
            var fileClient = rootDir.GetFileClient(fileName);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }

        public async Task<Stream?> DownloadFileAsync(string fileName)
        {
            var rootDir = _shareClient.GetRootDirectoryClient();
            var fileClient = rootDir.GetFileClient(fileName);
            var response = await fileClient.DownloadAsync();
            return response.Value.Content;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var rootDir = _shareClient.GetRootDirectoryClient();
            var fileClient = rootDir.GetFileClient(fileName);
            await fileClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> ListFilesAsync()
        {
            var rootDir = _shareClient.GetRootDirectoryClient();
            var files = new List<string>();

            await foreach (var item in rootDir.GetFilesAndDirectoriesAsync())
            {
                if (!item.IsDirectory)
                    files.Add(item.Name);
            }

            return files;
        }
    }
}
