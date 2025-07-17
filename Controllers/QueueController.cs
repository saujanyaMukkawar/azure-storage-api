using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues.Models;
using AzureStorageApi.Services;

[ApiController]
[Route("api/[controller]")]
public class QueueController : ControllerBase
{
    private readonly QueueService _queueService;

    public QueueController(QueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] string message)
    {
        await _queueService.SendMessageAsync(message);
        return Ok("Message sent");
    }

    [HttpGet("receive")]
    public async Task<IActionResult> ReceiveMessage()
    {
        var message = await _queueService.ReceiveMessageAsync();
        if (message == null)
            return NotFound("No messages in queue");

        return Ok(new
        {
            message.MessageId,
            message.MessageText,
            message.PopReceipt
        });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteMessage([FromQuery] string messageId, [FromQuery] string popReceipt)
    {
        await _queueService.DeleteMessageAsync(messageId, popReceipt);
        return Ok("Message deleted");
    }

    [HttpGet("peek")]
    public async Task<IActionResult> PeekMessages()
    {
        var messages = await _queueService.PeekMessagesAsync();
        return Ok(messages);
    }
}
