using Microsoft.AspNetCore.Mvc;
using AzureStorageApi.Services;
using AzureStorageApi.Models;

[ApiController]
[Route("api/[controller]")]
public class FileShareController : ControllerBase
{
    private readonly FileShareService _fileShareService;

    public FileShareController(FileShareService fileShareService)
    {
        _fileShareService = fileShareService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("File is empty");

        using var stream = request.File.OpenReadStream();
        await _fileShareService.UploadFileAsync(request.File.FileName, stream);
        return Ok("File uploaded");
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> DownloadFile(string fileName)
    {
        var stream = await _fileShareService.DownloadFileAsync(fileName);
        if (stream == null)
            return NotFound("File not found");

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> DeleteFile(string fileName)
    {
        await _fileShareService.DeleteFileAsync(fileName);
        return Ok("File deleted");
    }

    [HttpGet]
    public async Task<IActionResult> ListFiles()
    {
        var files = await _fileShareService.ListFilesAsync();
        return Ok(files);
    }
}
