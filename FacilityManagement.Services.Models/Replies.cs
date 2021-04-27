using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.Models
{
    public class Replies : BaseEntity
    {
        [StringLength(500)]
        public string Reply { get; set; }
        public string UserId { get; set; } 
        public string CommentId { get; set; }
        //navigational property
        public User User { get; set; }
        public Comments Comments { get; set; }
    }
}
