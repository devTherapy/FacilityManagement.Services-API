using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class EditRatingDTO
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
