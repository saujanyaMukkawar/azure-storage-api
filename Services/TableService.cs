using Azure;
using Azure.Data.Tables;

namespace AzureStorageApi.Services
{
    public class TableService
    {
        private readonly TableClient _tableClient;

        public TableService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:Table:ConnectionString"];
            var tableName = config["AzureStorage:Table:TableName"];
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task AddEntityAsync(CustomerEntity entity)
        {
            await _tableClient.AddEntityAsync(entity);
        }

        public async Task<CustomerEntity?> GetEntityAsync(string rowKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<CustomerEntity>("Customer", rowKey);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateEntityAsync(CustomerEntity entity)
        {
            await _tableClient.UpdateEntityAsync(entity, ETag.All, TableUpdateMode.Replace);
        }

        public async Task DeleteEntityAsync(string rowKey)
        {
            await _tableClient.DeleteEntityAsync("Customer", rowKey);
        }

        public async Task<List<CustomerEntity>> GetAllEntitiesAsync()
        {
            var entities = new List<CustomerEntity>();
            await foreach (var entity in _tableClient.QueryAsync<CustomerEntity>())
            {
                entities.Add(entity);
            }
            return entities;
        }
    }
}
