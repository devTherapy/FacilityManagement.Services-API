using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.TestsForUserController
{
    class GetUsersByNameEndpoint
    {
        private UserController _userController;
        private IServiceProvider _serviceProvider;
        private Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        private Mock<UserManager<User>> _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        private Response<Pagination<UserDTO>> _pagedUsers = Helper.GetPaginatedUsers;
        private Response<Pagination<UserDTO>> _invalidPagedUsers = Helper.GetPaginatedUsersInvalid;

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider.Setup(x => x.GetService(typeof(IUserService))).Returns(_userServiceMock.Object).Verifiable();
            mockServiceProvider.Setup(x => x.GetService(typeof(UserManager<User>))).Returns(_userManagerMock.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;

            _userController = new UserController(_serviceProvider);
        }
        [Test]
        public async Task GetUsersByNameShouldReturnOkWhenSuccessful()
        {
            //Arrange
            var page = 1;
            var name = "aaa";
            _userServiceMock.Setup(i => i.GetUsersByName(page, name)).ReturnsAsync(_pagedUsers);

            //Act
            var result = await _userController.GetUsersByName(page, name) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsInstanceOf<Response<Pagination<UserDTO>>>(result.Value);
            _userServiceMock.Verify(i => i.GetUsersByName(page, name), Times.Once);
        }

        [Test]
        public async Task GetUsersByNameShouldReturnBadRequestWhenInvalid()
        {
            //Arrange
            var page = 1;
            var name = "bbb";
            _userServiceMock.Setup(i => i.GetUsersByName(page, name)).ReturnsAsync(_invalidPagedUsers);

            //Act
            var result = await _userController.GetUsersByName(page, name) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result.Value);
            Assert.True(result is BadRequestObjectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
            _userServiceMock.Verify(i => i.GetUsersByName(page, name), Times.Once);
        }
    }
}
