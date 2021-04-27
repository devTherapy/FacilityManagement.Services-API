using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace FacilityManagement.Services.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string AvatarUrl { get; set; }
        public string PublicId { get; set; }
        public string Squad { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }

        // navigational properties

        public ICollection<Comments> Comments { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<Replies> Replies { get; set; }
        public ICollection<Ratings> Ratings { get; set; }
    }
}
