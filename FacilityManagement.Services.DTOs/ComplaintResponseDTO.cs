using FacilityManagement.Services.Models;
using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs
{
    public class ComplaintResponseDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }

        public bool IsTask { get; set; }
        public string UserId { get; set; }
        public string CategoryId { get; set; }

        // naviagtional properties
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
    }
}
