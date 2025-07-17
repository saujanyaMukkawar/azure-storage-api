using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureStorageApi.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration config)
        {
            var connectionString = config["AzureStorage:Queue:ConnectionString"];
            var queueName = config["AzureStorage:Queue:QueueName"];
            
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentException("Queue connection string or queue name is missing in configuration.");
            }
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }

        public async Task<QueueMessage?> ReceiveMessageAsync()
        {
            var response = await _queueClient.ReceiveMessagesAsync(maxMessages: 1);
            return response.Value.FirstOrDefault();
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }

        public async Task<List<string>> PeekMessagesAsync(int count = 5)
        {
            var messages = new List<string>();
            var response = await _queueClient.PeekMessagesAsync(count);
            foreach (var msg in response.Value)
            {
                messages.Add(msg.MessageText);
            }
            return messages;
        }
    }
}