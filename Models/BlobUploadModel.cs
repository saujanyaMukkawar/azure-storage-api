using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AzureStorageApi.Models
{
    public class BlobUploadModel
    {
        public required IFormFile File { get; set; }
    }
    
    public class FileUploadRequest
    {
        [Required]
        public required IFormFile File { get; set; }
    }

}
