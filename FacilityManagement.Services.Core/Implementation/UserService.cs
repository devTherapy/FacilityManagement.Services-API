using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;

        public UserService(IServiceProvider provider)
        {
            _userManager = provider.GetRequiredService<UserManager<User>>();
            _imageService = provider.GetRequiredService<IImageService>();
            _userRepository = provider.GetRequiredService<IUserRepository>();
        }

        public async Task<Response<ImageAddedDTO>> ChangePicture(User user, AddImageDTO model)
        {
            Response<ImageAddedDTO> response = new Response<ImageAddedDTO>();
            var result = await _imageService.UploadImage(model.Image);
            string publicId = result.PublicId;
            string url = result.Url.ToString();

            if (publicId == null || url == null)
            {
                response.Message = "An error occured while trying to upload your image";
                return response;
            }

            var userToUpdate = await _userManager.FindByEmailAsync(user.Email);

            if(userToUpdate is null)
            {
                response.Message = "Could not find user";
                return response;
            }

            userToUpdate.PublicId = publicId;
            userToUpdate.AvatarUrl = url;

            var userResult = await _userManager.UpdateAsync(userToUpdate);

            if (userResult.Succeeded)
            {
                var responseDTO = new ImageAddedDTO
                {
                    PublicId = publicId,
                    Url = url
                };

                response.Message = "Image Uploaded successfully!";
                response.Success = true;
                response.Data = responseDTO;
            }
            else
                response.Message = "Could not upload image";

            return response;
        }
        
        public async Task<Response<string>> ChangePassword(User user, ChangePasswordDTO model)
        {
            Response<string> response = new Response<string>();
            if (model == null)
            {
                response.Message = "All fields are required";
                return response;
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                response.Message = "Confirm password doesn't match new password";
                return response;
            }

            var appUser = await _userManager.FindByEmailAsync(user.Email);

            var result = await _userManager.ChangePasswordAsync(appUser, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                response.Message = "user password changed successfully";
                response.Success = true;
            }
            else
                response.Message = "Invalid credentials";

            return response;
        }

        public async Task<Response<string>> UpdateUser(User user, UpdateUserDTO model)
        {
            Response<string> response = new Response<string>();
            if (user == null)
            {
                response.Message = "Sorry! You cannot perform this operation";
                return response;
            }
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.UserName = model.UserName ?? user.UserName;
            user.Gender = model.Gender ?? user.Gender;
            user.Squad = model.Squad ?? user.Squad;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;
            user.IsProfileCompleted = true;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                response.Message = "Update Successful";
                response.Success = true;
            }
            else
                response.Message = "Could not update user";

            return response;
        }

        public async Task<Response<UserDTO>> GetUser(string id)
        {
            Response<UserDTO> response = new Response<UserDTO>();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Message = "Sorry! You cannot find user";
                return response;
            }

            var dto = MapToUserDTO.ToUserDTO(user);

            if (dto != null)
            {
                response.Message = "User gotten";
                response.Success = true;
                response.Data = dto;
            }
            else
                response.Message = "could not get user data";

            return response;
        }

        public async Task<Response<string>> ActivateUser(string id)
        {
            Response<string> response = new Response<string>();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Message = "cannot find user";
                return response;
            }
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "user activated";
            }
            else
                response.Message = "Could not activate user";

            return response;
        }

        public async Task<Response<string>> DeactivateUser(string id)
        {
            Response<string> response = new Response<string>();
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                response.Message = "cannot find user";
                return response;
            }
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                response.Success = true;
                response.Message = "user deactivated";
            }
            else
                response.Message = "user could not be deactivated";

            return response;
        }

        public async Task<Response<Pagination<UserDTO>>> GetUsers(int pageNumber)
        {
            Response<Pagination<UserDTO>> response = new Response<Pagination<UserDTO>>();
            var users = await _userRepository.GetUsers(pageNumber);

            if (users == null)
            {
                response.Message ="No users on this page";
                return response;
            }

            var pagedResult = GetPaginatedUsersDtos(pageNumber, users);

            if (pagedResult is null)
                response.Message = "Could not fetch users";
            else
            {
                response.Success = true;
                response.Message = "Fetched users per page";
                response.Data = pagedResult;
            }

            return response;
        }

        public async Task<Response<Pagination<UserDTO>>> GetUsersBySquad(int pageNumber, string squad)
        {
            Response<Pagination<UserDTO>> response = new Response<Pagination<UserDTO>>();
            var users = await _userRepository.GetUsersBySquad(squad, pageNumber);

            if (users == null)
            {
                response.Message = "No users on this page";
                return response;
            }
            var pagedResult= GetPaginatedUsersDtos(pageNumber, users);
            
            if(pagedResult != null)
            {
                response.Success = true;
                response.Message = "fetched Users in squad per page";
                response.Data = pagedResult;
                return response;
            }

            response.Message = "Could not fetch users in the squad provided";
            return response;
        }

        public async Task<Response<Pagination<UserDTO>>> GetUsersByName(int pageNumber, string name)
        {
            Response<Pagination<UserDTO>> response = new Response<Pagination<UserDTO>>();
            var users = await _userRepository.GetUsersByName(name, pageNumber);

            if (users is null)
            {
                response.Message = "no users on this page";
                return response;
            }

            var pagedResult = GetPaginatedUsersDtos(pageNumber, users);

            if(pagedResult != null)
            {
                response.Success = true;
                response.Message = "fetched users with name per page";
                response.Data = pagedResult;
                return response;
            }

            response.Message = "Could not fetch users with name given";
            return response;
        }

        private Pagination<UserDTO> GetPaginatedUsersDtos(int pageNumber, ICollection<User> users)
        {

            ICollection<UserDTO> mapUsersToDto = PaginationMappers.ForUsers(users);

            return new Pagination<UserDTO>
            {
                TotalNumberOfItems = _userRepository.TotalNumberOfItems,
                TotalNumberOfPages = _userRepository.TotalNumberOfPages,
                Items = mapUsersToDto,
                ItemsPerPage = _userRepository.perPage,
                CurrentPage = pageNumber
            };
        }

        public async Task<Response<string>> ChangeUserRole(string userId, ChangeRoleDto model)
        {
            var response = new Response<string>();
            var user = await _userManager.FindByIdAsync(userId);
            var currentRoles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                response.Message = ($"user with {user.Id} could not be found");
                return response;
            }

            if (!(await _userManager.IsInRoleAsync(user, model.Role)))
            {
                await _userManager.RemoveFromRoleAsync(user, currentRoles.Where(x => x == model.CurrentRole).ToString());
                await _userManager.AddToRoleAsync(user, model.Role);
                response.Message = "user role has been changed";
                response.Success = true;
            }
            else
                response.Message = "user already has this role";

            return response;
        }
    }
}
