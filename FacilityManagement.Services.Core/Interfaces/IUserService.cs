using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IUserService
    {
        Task<Response<ImageAddedDTO>> ChangePicture(User user, AddImageDTO model);
        Task<Response<string>> ChangePassword(User user, ChangePasswordDTO model);
        Task<Response<string>> UpdateUser(User user, UpdateUserDTO model);
        Task<Response<string>> ChangeUserRole(string userId, ChangeRoleDto model);
        Task<Response<Pagination<UserDTO>>> GetUsersByName(int pageNumber, string name);
        Task<Response<Pagination<UserDTO>>> GetUsersBySquad(int pageNumber, string squad);
        Task<Response<Pagination<UserDTO>>> GetUsers(int pageNumber);
        Task<Response<UserDTO>> GetUser(string id); 
        Task<Response<string>> ActivateUser(string id);
        Task<Response<string>> DeactivateUser(string id);
    }
}
