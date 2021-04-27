using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class ResetPasswordDTO
    {

        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password must match.")]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }

    }
}
