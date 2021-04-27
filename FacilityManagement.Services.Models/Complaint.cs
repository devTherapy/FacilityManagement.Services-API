using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.Models
{
    public class Complaint: BaseEntity
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
        public string CategoryId { get; set; }

        // naviagtional properties
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
    }
}
