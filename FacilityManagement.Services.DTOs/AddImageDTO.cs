using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class AddImageDTO
    {
        [Required]
        public IFormFile Image { get; set; }
    }
}
