using FacilityManagement.Services.Models;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.DTOs
{
    public class AddComplaintDTO
    {
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Question { get; set; }

        [StringLength(500)]
        public string Type { get; set; }

        public string Image { get; set; }

        public bool IsTask { get; set; }
        public string UserId { get; set; }
    }
}
