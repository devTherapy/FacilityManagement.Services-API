using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FacilityManagement.Common.Utilities.Helpers;

namespace FacilityManagement.Services.DTOs
{
    /// <summary>
    /// DTO for external login details with SSO
    /// </summary>
    public class ExternalLoginSsoDTO
    {
            [Required]
            [JsonPropertyName("givenName")]
            public string FirstName { get; set; }

            [Required]
            [JsonPropertyName("surname")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [ValidEmailDomain(ErrorMessage = "Invalid organization email")]
            [JsonPropertyName("mail")]
            public string Email { get; set; }
    }
}
