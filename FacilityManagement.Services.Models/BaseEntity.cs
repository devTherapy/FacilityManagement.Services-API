using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FacilityManagement.Services.Models
{
    public abstract class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; } 
    }
}
