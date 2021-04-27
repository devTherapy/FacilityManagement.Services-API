using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Http;
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
    public class OkTest
    {
        private IServiceProvider _serviceProvider;
        private Mock<IAuthService> mockAuthService = new Mock<IAuthService>();
        private IUrlHelper _urlHelper;

        [SetUp]
        public void Setup()
        {            
           
            //Mock ImageUpload service
            var mockImageUpload = new Mock<IImageService>();

            //Mock IConfiguration
            var mockConfig = new Mock<IConfiguration>(MockBehavior.Strict);

            //Mock GenerateJwt Token
            var mockGenerateJwt = new Mock<IJWTService>(MockBehavior.Strict);

            //Mock ILogger
            var mockILogger = new Mock<ILogger>(MockBehavior.Strict);
            mockILogger.Setup(x => x.Error(It.IsAny<string>())).Verifiable();

            //Mock UrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl")
                .Verifiable();

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

            //Mock Mailing service
            Mock<IMailService> mockMailService = new Mock<IMailService>();
            mockMailService.Setup(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

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

        [Test]
        public async Task SendInviteTestValid()
        {
            //Arrange
            MockUp(true);
            var authController = new AuthController(_serviceProvider)
            {
                Url = _urlHelper
            };

            authController.ControllerContext.HttpContext = new DefaultHttpContext();
            var userToInvite = new InviteReturnDTO();

            //Act
            var actual = await authController.SendInvite(userToInvite) as CreatedResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status201Created, actual.StatusCode);
        }

        [Test]
        public async Task SendInviteTestInValid()
        {
            //Arrange
            MockUp(false);
            var authController = new AuthController(_serviceProvider)
            {
                Url = _urlHelper
            };

            authController.ControllerContext.HttpContext = new DefaultHttpContext();
            var userToInvite = new InviteReturnDTO();

            //Act
            var actual = await authController.SendInvite(userToInvite) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            mockAuthService.Setup(method => method.InviteUser(It.IsAny<InviteReturnDTO>(), It.IsAny<IUrlHelper>(), It.IsAny<string>()))
               .Returns(Task.FromResult(new Response<List<InviteResponseDTO>> { Success = state }));
        }
    }
}