using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class FileUploadDTO
    {
        [Required]
        public string Foldername { get; set; } = string.Empty;

        [Required]
        public IFormFile File { get; set; } = default!;
    }
}
