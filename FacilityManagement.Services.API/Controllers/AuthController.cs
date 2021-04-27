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
    /// Authentication Controller
    /// </summary>
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// AuthController constructor
        /// </summary>
        public AuthController(IServiceProvider provider)
        {
            _authService = provider.GetRequiredService<IAuthService>();
        }

        /// <summary>
        /// Allows the admin user to send invites to a set of users with a given role by their email address 
        /// logs in a user 
        /// </summary>
        /// <param name="userInvite"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpPost("send-invite")]
        public async Task<IActionResult> SendInvite([FromBody] InviteReturnDTO userInvite)
        {
            var response = await _authService.InviteUser(userInvite, Url, Request.Scheme);
            if (response.Success) return Created("", response);
            return BadRequest(response);
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDetailsDTO model)
        {
            var result = await _authService.Login(model);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Logs in a user through data gotten from Microsoft SSO
        /// </summary>
        /// <param name="bearer"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("external-login")]
        public async Task<IActionResult> ExternalLogin([FromHeader] string bearer)
        {
            var result = await _authService.ExternalLogin(bearer);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// sends a email to a user to user to reset the forgotten password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            var result = await _authService.ForgotPassword(model.Email, Url, Request.Scheme);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Implements a reset password functionality for user that has forgotten his/her password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var result = await _authService.ResetPassword(model);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}