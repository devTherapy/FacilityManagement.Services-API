using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.Models
{
    public class Comments : BaseEntity
    {
        [StringLength(500)]
        public string Comment { get; set; }

        public string UserId { get; set; } 
        public string ComplaintId { get; set; }

        //navigational property
        public ICollection<Replies> Replies { get; set; }
        public User User { get; set; }
        public Complaint Complaints { get; set; }

    }
}
