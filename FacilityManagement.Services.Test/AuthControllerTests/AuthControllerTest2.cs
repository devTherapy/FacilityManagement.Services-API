using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    /// <summary>
    /// Unit Testing for Facility Management App
    /// </summary>
    public class Tests
    {
        private IServiceProvider _serviceProvider;

        private IUrlHelper _urlHelper;

        [SetUp]
        public void Setup()
        {
            //Mock IAuthService
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(method => method.InviteUser(It.IsAny<InviteReturnDTO>(), It.IsAny<IUrlHelper>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new Response<List<InviteResponseDTO>> { Success = false }));
            //Mock Usermanager
            var store = new Mock<IUserStore<User>>();
            List<User> mockDataBase = new List<User>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).
               Returns(Task.FromResult(mockDataBase.Find(user => user.Email == It.IsAny<string>())));
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<string>());

            //MockRoleManager
            List<string> roles = new List<string>() { "Admin", "Client" };
            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            mockRoleManager.Setup(role => role.RoleExistsAsync(It.IsAny<string>())).
                Returns(Task.FromResult(true));

            //Mock ImageUpload service
            var mockImageUpload = new Mock<IImageService>();

            //Mock GenerateJwt Token
            var mockGenerateJwt = new Mock<IJWTService>(MockBehavior.Strict);

            //Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>(MockBehavior.Strict);

            //Mock ILogger
            var mockILogger = new Mock<ILogger>(MockBehavior.Strict);
            mockILogger.Setup(x => x.Error(It.IsAny<string>())).Verifiable();

            //Mock UrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();

            //Mock IServiceProvider
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(x => x.GetService(typeof(IAuthService))).Returns(mockAuthService.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(IJWTService))).Returns(mockGenerateJwt.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(IImageService))).Returns(mockImageUpload.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(ILogger))).Returns(mockILogger.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(IConfiguration))).Returns(mockConfig.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(UserManager<User>))).Returns(mockUserManager.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(RoleManager<IdentityRole>))).Returns(mockRoleManager.Object).Verifiable();

            //Assign data
            _serviceProvider = mockServiceProvider.Object;
            _urlHelper = mockUrlHelper.Object;
        }

       
    }
}