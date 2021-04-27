using System;

namespace FacilityManagement.Services.DTOs
{
    /// <summary>
    /// cClass definition for a user DTO
    /// </summary>
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsProfileCompleted { get; set; }
        public string AvatarUrl { get; set; }
        public string Squad { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
    }
}