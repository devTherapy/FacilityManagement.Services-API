using System;
using System.Threading.Tasks;
using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FacilityManagement.Services.Test
{
    public class ActivateUserTest
    {
        public IServiceProvider _serviceProvider;
        public static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        public  Mock<UserManager<User>> mockUserManager { get; set; } = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        public Mock<IUserService> mockUserService { get; set; } = new Mock<IUserService>();

        [SetUp]
        public void SetUp()
        {

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(UserManager<User>))).
                Returns(mockUserManager.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).
               Returns(mockUserService.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestActivateUserVald()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(true);
            var expected = 204;

            //Act
            var actual = await userController.ActivateUser("") as NoContentResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);


            
        }
        [Test]
        public async Task TestActivateUserInvalid()
        {
            //Arrange
            var userController = new UserController(_serviceProvider);
            MockReturn(false);
            var expected = 400;

            //Act
            var actual = await userController.ActivateUser("") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        public void MockReturn(bool state)
        {
            mockUserService.Setup(x => x.ActivateUser(It.IsAny<string>())).Returns(Task.FromResult(new Response<string> { Success = state }));
        }
    }
}
