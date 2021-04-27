using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class InviteReturnDTO
    {
        [Required]
        public IEnumerable<string> EmailAddresses { get; set; }

        [Required]
        public string Role { get; set; }
    }
}