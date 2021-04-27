using FacilityManagement.Services.Models;
using System.Collections.Generic;

namespace FacilityManagement.Services.DTOs
{
    public class ComplaintsDTO
    {
        public string Title { get; set; }

        public string Question { get; set; }

        public string Type { get; set; }

        public string Image { get; set; }

        public bool IsTask { get; set; }
        public ICollection<ComplaintCommentsDTO> Comments { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
        public ComplaintUserDTO User { get; set; }
    }
}
