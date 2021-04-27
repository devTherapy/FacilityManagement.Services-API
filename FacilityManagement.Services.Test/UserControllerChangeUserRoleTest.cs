using System;
using System.Collections.Generic;
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
    public class UserControllerChangeUserRoleTest
    {
        private IServiceProvider _serviceProvider;
        public Mock<IUserService> MockUserService { get; set; } = new Mock<IUserService>();

        [SetUp]
        public void SetUp()
        {
            //Mock Usermanager
            var store = new Mock<IUserStore<User>>();
            List<User> mockDataBase = new List<User>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            //mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).
               // Returns(Task.FromResult(mockDataBase.Find(user => user.Id == It.IsAny<string>())));
            //mockUserManager.Setup(x => x.IsInRoleAsync(It.IsAny<User>(),It.IsAny<string>())).ReturnsAsync(It.IsAny<bool>());
            //mockUserManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            //mockUserManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            //mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(It.IsAny<IList<string>>());

            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IUserService))).Returns(MockUserService.Object).Verifiable();
            mockServiceProvider.Setup(get => get.GetService(typeof(UserManager<User>))).Returns(mockUserManager.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task ShouldReturnOkResultWhenValidModelPassed()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var model = new ChangeRoleDto();
            MockUserService.Setup(x => x.ChangeUserRole(It.IsAny<string>(), It.IsAny<ChangeRoleDto>()))
                .Returns(Task.FromResult(new Response<string>() { Success = true }));
            var userController = new UserController(_serviceProvider);

            var expected = 200;

            //ACT
            var actual = await userController.ChangeRole(id,model) as OkObjectResult;

            //Assert
            //Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
    }

    
}
