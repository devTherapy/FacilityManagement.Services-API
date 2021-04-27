using FacilityManagement.Services.API.Policy;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.API.Controllers
{
    /// <summary>
    /// UserController class
    /// </summary>
    public class UserController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// UserController constructor
        /// </summary>
        public UserController(IServiceProvider provider)
        {
            _userService = provider.GetRequiredService<IUserService>();
            _userManager = provider.GetRequiredService<UserManager<User>>();
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpGet("get-users/{pageNumber}")]
        public async Task<IActionResult> GetUsers(int pageNumber)
        {
            var result = await _userService.GetUsers(pageNumber);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Gets a User by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("get-user/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var result = await _userService.GetUser(userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Get all users in a specific squad
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="squad"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpGet("search-by-squad/{squad}/{pageNumber}")]
        public async Task<IActionResult> GetUsersInSquad(int pageNumber, string squad)
        {
            var result = await _userService.GetUsersBySquad(pageNumber, squad);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Get all users by name
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpGet("search-by-name/{name}/{pageNumber}")]
        public async Task<IActionResult> GetUsersByName(int pageNumber, string name)
        {
            var result = await _userService.GetUsersByName(pageNumber, name);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Updates a User in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("update-profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userService.UpdateUser(user, model);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        /// <summary>
        /// Implements image upload using cloudinary
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("change-picture")]
        public async Task<IActionResult> ChangePicture([FromForm] AddImageDTO model)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var result = await _userService.ChangePicture(user, model);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// change the role of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpPatch("change-role/({userId})")]
        public async Task<IActionResult> ChangeRole(string userId, [FromBody] ChangeRoleDto model)
        {
            var result = await _userService.ChangeUserRole(userId, model);
            if (result.Success) return Ok(result);
           return BadRequest(result);
        }

        /// <summary>
        /// Activate a User in the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpPatch("activate-user/{userId}")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var result = await _userService.ActivateUser(userId);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        /// <summary>
        /// Deactivates a User in the database
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Policy =Policies.Admin)]
        [HttpPatch("deactivate-user/{userId}")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var result = await _userService.DeactivateUser(userId);
            if(result.Success) return NoContent();
            return BadRequest(result);          
        }

        /// <summary>
        /// Implements a logged in user's change password functionality
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Vendor)]
        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var result = await _userService.ChangePassword(appUser, model);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
