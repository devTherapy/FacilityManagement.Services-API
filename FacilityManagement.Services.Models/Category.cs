using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacilityManagement.Services.Models
{
    public class Category : BaseEntity
    {
        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public ICollection<Complaint> Complaints { get; set; }
    }
}
