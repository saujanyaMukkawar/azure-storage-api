using AzureStorageApi.Models;
using AzureStorageApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BlobController : ControllerBase
{
    private readonly BlobService _blobService;

    public BlobController(BlobService blobService)
    {
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<IActionResult> ListBlobs()
    {
        var blobs = await _blobService.ListBlobsAsync();
        return Ok(blobs);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadBlob([FromForm] BlobUploadModel model)
    {
        if (model.File == null || model.File.Length == 0)
            return BadRequest("File is empty");

        using var stream = model.File.OpenReadStream();
        await _blobService.UploadBlobAsync(model.File.FileName, stream);
        return Ok("Uploaded successfully");
    }

    [HttpGet("{blobName}")]
    public async Task<IActionResult> DownloadBlob(string blobName)
    {
        var stream = await _blobService.DownloadBlobAsync(blobName);
        return File(stream, "application/octet-stream", blobName);
    }

    [HttpDelete("{blobName}")]
    public async Task<IActionResult> DeleteBlob(string blobName)
    {
        await _blobService.DeleteBlobAsync(blobName);
        return Ok("Deleted successfully");
    }
}
