using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.Models
{
    public class Ratings : BaseEntity
    {
        [Range(1,5)]
        public int Rating { get; set; } 

        public string UserId { get; set; }

        public string ComplaintId { get; set; }

        //navigational property
        public User User { get; set; }
        public Complaint Complaint { get; set; }
    }
}
