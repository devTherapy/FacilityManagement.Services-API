using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public class MapToUserDTO
    {
        public static UserDTO ToUserDTO(User user)
        {
            var dto = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Gender = user.Gender,
                IsActive = user.IsActive,
                IsProfileCompleted = user.IsProfileCompleted,
                AvatarUrl = user.AvatarUrl,
                Squad = user.Squad,
                PhoneNumber = user.PhoneNumber,
                Created_at = user.Created_at,
                Updated_at = user.Updated_at



            };
            return dto;
        }
    }
}
