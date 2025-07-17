using AzureStorageApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TableController : ControllerBase
{
    private readonly TableService _tableService;

    public TableController(TableService tableService)
    {
        _tableService = tableService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CustomerEntity entity)
    {
        await _tableService.AddEntityAsync(entity);
        return Ok("Entity created");
    }

    [HttpGet("{rowKey}")]
    public async Task<IActionResult> Get(string rowKey)
    {
        var entity = await _tableService.GetEntityAsync(rowKey);
        if (entity == null)
            return NotFound("Entity not found");

        return Ok(entity);
    }

    [HttpPut("{rowKey}")]
    public async Task<IActionResult> Update(string rowKey, [FromBody] CustomerEntity updated)
    {
        updated.RowKey = rowKey;
        updated.PartitionKey = "Customer";
        await _tableService.UpdateEntityAsync(updated);
        return Ok("Entity updated");
    }

    [HttpDelete("{rowKey}")]
    public async Task<IActionResult> Delete(string rowKey)
    {
        await _tableService.DeleteEntityAsync(rowKey);
        return Ok("Entity deleted");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entities = await _tableService.GetAllEntitiesAsync();
        return Ok(entities);
    }
}
