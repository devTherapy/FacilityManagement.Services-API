using FacilityManagement.Common.Utilities.Helpers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly IJWTService _jWTService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailServices;
        private readonly IGraphApiService _graphApiService;
        private Dictionary<string, string> _data;
        private readonly string path = "../FacilityManagement.Services.API";

        public AuthService(IServiceProvider provider)
        {
            _mailServices = provider.GetRequiredService<IMailService>();
            _roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            _configuration = provider.GetRequiredService<IConfiguration>();
            _userManager = provider.GetRequiredService<UserManager<User>>();
            _jWTService = provider.GetRequiredService<IJWTService>();
            _graphApiService = provider.GetRequiredService<IGraphApiService>();
            _data = new Dictionary<string, string>();
        }

        public async Task<Response<LoginResponseDTO>> Login(LoginDetailsDTO model)
        {
            Response<LoginResponseDTO> response = new Response<LoginResponseDTO>();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var token = _jWTService.GetToken(user, _configuration, userRoles);

                response.Success = true;
                response.Message = "Login Successful";
                response.Data = new LoginResponseDTO
                {
                    Token = token,
                    IsProfileCompleted = user.IsProfileCompleted,
                    UserId = user.Id,
                };

                return response;
            }

            response.Message = "Invalid login details";
            return response;
        }

        public async Task<Response<string>> ForgotPassword(string email, IUrlHelper url, string requestScheme)
        {
            Response<string> response = new Response<string>();

            if (string.IsNullOrEmpty(email))
            {
                response.Message = "Please provide an email";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.Message = "email does not exist";
                return response;
            }

            var token = await EmailForgotPasswordToken(user);
            var temp = await GetForgotPasswordTemplatePath();

            var passwordResetLink = url.Action("ResetPassword", "Auth", new { email, token }, requestScheme);

            var newTemp = temp.Replace("**resetPasswordLink**", passwordResetLink);
            var newestTemp = newTemp.Replace("**User**", user.FirstName);

            var mailSent = await _mailServices.SendEmail(email, newestTemp, "ResetPassword");

            if (mailSent)
            {
                response.Success = true;
                response.Message = "Link sent to the email successfully";
                return response;
            }

            response.Message = "Mail failed to send";
            return response;
        }

        private async Task<string> EmailForgotPasswordToken(User user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);
            return validToken;
        }

        private async Task<string> GetForgotPasswordTemplatePath()
        {
            var temp = await File.ReadAllTextAsync(Path.Combine(path, "StaticFiles/ForgotPassword.html"));
            return temp;
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordDTO model)
        {
            Response<string> response = new Response<string>
            {
                Success = false
            };
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response.Message = "No user associated with email";
                return response;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                response.Message = "Password doesn't match its confirmation";
                return response;
            }

            var decodedToken = WebEncoders.Base64UrlDecode(model.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);

            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "Password has been reset successfully!";
                return response;
            }

            response.Message = "Something went wrong";
            return response;
        }

        public async Task<Response<ExternalLoginResponseDTO>> ExternalLogin(string token)
        {
            Response<ExternalLoginResponseDTO> response = new Response<ExternalLoginResponseDTO>
            {
                Success = false
            };
            var apiResponse = await _graphApiService.GetAuthorizedUserDetails(token);

            if (apiResponse.externalLoginUser == null)
            {
                response.Message = apiResponse.Error;
                return response;

            }
            var externalUser = apiResponse.externalLoginUser;
            var user = await _userManager.FindByEmailAsync(externalUser.Email);

            if (user == null)
            {
                var isUserRegistered = await RegisterUser(externalUser);
                if (!isUserRegistered)
                {
                    response.Message = "Unable to log in user";
                    return response;
                }
                user = await _userManager.FindByEmailAsync(externalUser.Email);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            response.Success = true;
            response.Message = "User is logged in";
            response.Data = new ExternalLoginResponseDTO
            {
                Id = user.Id,
                IsProfileCompleted = user.IsProfileCompleted,
                Token = _jWTService.GetToken(user, _configuration, userRoles),
            };

            return response;
        }

        private async Task<bool> RegisterUser(ExternalLoginSsoDTO model)
        {
            var isUserCreated = await CreateUser(model);

            if (!isUserCreated) return false;
            var user = await _userManager.FindByEmailAsync(model.Email);
            var isRoleAdded = await AddRoleToUser(user);

            if (isRoleAdded) return true;
            await _userManager.DeleteAsync(user);
            return false;
        }

        private async Task<bool> CreateUser(ExternalLoginSsoDTO model)
        {
            var newUser = new User
            {
                Email = model.Email,
                IsProfileCompleted = false,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(newUser);
            return result.Succeeded;
        }

        private async Task<bool> AddRoleToUser(User user)
        {
            var emailDomain = Utilities.GetEmailDomain(user.Email);

            switch (emailDomain)
            {
                case "decagon.dev":
                    var result = await _userManager.AddToRoleAsync(user, "decadev");
                    return result.Succeeded;
                // TODO:  you can add other email domains that determine the role to be added to the user(remember to also add to ValidEmailDomain)
                default:
                    return false;
            }
        }

        private async Task<bool> IsValidRole(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }

        private async Task<string> CreateUserName(string email)
        {
            var username = email.Split('@')[0];
            username = await _userManager.FindByNameAsync(username) == null ? username :
                username += Guid.NewGuid().ToString().Substring(4, 9);
            return username;
        }

        private async Task DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            await _userManager.DeleteAsync(user);
        }

        private async Task<TokenRouteErrors> CreateUserAndPassword(IEnumerable<string> emails, string role, Dictionary<string, string> data)
        {
            var result = new TokenRouteErrors { Data = data };
            foreach (var email in emails)
            {
                var username = await CreateUserName(email);

                var newUser = UserProfile.ToUser(email, username);
                var password = Guid.NewGuid().ToString();
                password = password.Replace('-', '@');
                var createdUser = await _userManager.CreateAsync(newUser, password);

                if (!createdUser.Succeeded)
                {
                    result.Data.Add(email, "NotSent");
                }
                else
                {
                    await _userManager.AddToRoleAsync(newUser, role);
                    result.UserAndPassword.Add(newUser, password);
                }
            }
            return result;
        }

        private async Task<EmailCheck> VerifyMail(IEnumerable<string> emails, Dictionary<string, string> data)
        {
            var result = new EmailCheck { Data = data };

            foreach (var email in emails)
            {
                if (MailValidations.IsMailValid(email))
                {
                    var checkUser = await _userManager.FindByEmailAsync(email);
                    if (checkUser != null)
                        result.Data.Add(email, "Email already exists");
                    else
                        result.Emails.Add(email);
                }
                else
                    result.Data.Add(email, "InvalidMail");
            }
            return result;
        }

        private async Task<Dictionary<string, string>> SendMail(Dictionary<User, string> userAndToken, IUrlHelper url, string requestScheme, string role, Dictionary<string, string> data)
        {
            foreach (var user in userAndToken)
            {

                var emailConfirmationLink = url.Action("Login", "Auth", new { Email = user.Key.Email, Password = user.Value }, requestScheme);

                //SentUp email Body
                var inviteA = File.ReadAllText(Path.Combine(path, "StaticFiles/InvitationA.html"));
                var password = string.Format("<p><strong> Your default password is {0} </strong></p>", user.Value);
                var linkInBetween = string.Format("<a href = {0} target ='_blank' style = 'color: aliceblue'> Accept Invite </a>", emailConfirmationLink);
                var inviteB = File.ReadAllText(Path.Combine(path, "StaticFiles/InvitationB.html"));
                var emailBody = inviteA + password + linkInBetween + inviteB;

                //Send the mail
                var result = await _mailServices.SendEmail(user.Key.Email, emailBody, "Decagon Facility Management App Invite");
                if (result) { data.Add(user.Key.Email, user.Key.Id); } else { await DeleteUser(user.Key.Email); data.Add(user.Key.Email, "NotSent"); }
            }

            return data;
        }

        public async Task<Response<List<InviteResponseDTO>>> InviteUser(InviteReturnDTO userInvite, IUrlHelper url, string requestScheme)
        {
            Response<List<InviteResponseDTO>> response = new Response<List<InviteResponseDTO>>();
            var roleCheck = await IsValidRole(userInvite.Role);
            if (!roleCheck)
            {
                response.Message = "Role is not valid";
                return response;
            }

            var VerifiedMails = await VerifyMail(userInvite.EmailAddresses, _data);
            _data = VerifiedMails.Data;

            if (VerifiedMails.Emails.Count == 0)
            {
                response.Data = ResponseMessageMapper.MapToResponseMsg(_data);
                response.Message = "Emails are Invalid";
                return response; 
            }

            var tokens = await CreateUserAndPassword(VerifiedMails.Emails, userInvite.Role, _data);
            _data = tokens.Data;

            _data = await SendMail(tokens.UserAndPassword, url, requestScheme, userInvite.Role, _data);

            response.Success = true;
            response.Data = ResponseMessageMapper.MapToResponseMsg(_data);
            response.Message = "Invites sent";
            return response;
        }
    }
}
