using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class LoginDetailsDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
