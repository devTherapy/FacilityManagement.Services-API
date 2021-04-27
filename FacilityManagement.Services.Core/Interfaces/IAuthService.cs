using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IAuthService
    {
        Task<Response<LoginResponseDTO>> Login(LoginDetailsDTO model);
        Task<Response<string>> ForgotPassword(string email, IUrlHelper url, string requestScheme);
        Task<Response<string>> ResetPassword(ResetPasswordDTO model);
        Task<Response<ExternalLoginResponseDTO>> ExternalLogin(string token);
        Task<Response<List<InviteResponseDTO>>> InviteUser(InviteReturnDTO userInvite, IUrlHelper url, string requestScheme);
    }
}
