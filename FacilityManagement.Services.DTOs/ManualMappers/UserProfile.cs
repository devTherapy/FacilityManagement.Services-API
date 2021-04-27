using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public static class UserProfile
    {
        public static User ToUser(string emailAddress, string userName)
        {
            return new User { Email = emailAddress, UserName = userName };

        }
    }
}