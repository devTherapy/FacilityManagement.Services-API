using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    class CommentServiceTestDeleteCommentById
    {
        private IServiceProvider _serviceProvider;
        public Mock<ICommentRepository> mockCommentRepo { get; set; } = new Mock<ICommentRepository>();
        public Mock<IComplaintRepository> mockComplaintRepo { get; set; } = new Mock<IComplaintRepository>();



        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(mockComplaintRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentRepository))).Returns(mockCommentRepo.Object).Verifiable();
            userManager.Setup(manager => manager.GetUserAsync(new ClaimsPrincipal())).Returns(Task.FromResult(new User()));
            _serviceProvider = mockServiceProvider.Object;

        }

        [Test]
        public async Task TestDeleteComplaintByIdValid()
        {
            //Arrange
            MockUp(new Comments(), new Complaint(), true);
            var commentServices = new CommentService(_serviceProvider);
            var expectedState = true;
            var expectedMessage = "Comment deleted successfully";

            //ACT
            var actual = await commentServices.DeleteComment("CommentId");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        [Test]
        public async Task TestDeleteCommentByIdInvalidComment()
        {
            //Arrange
            MockUp(null, new Complaint(), false);
            var commentServices = new CommentService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Invalid comment Id";

            //ACT
            var actual = await commentServices.DeleteComment("CommentId");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }


        [Test]
        public async Task TestDeleteCommmentByIdEdgeCase()
        {
            //Arrange
            MockUp(new Comments(), new Complaint(), false);
            var commentServices = new CommentService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Unable to delete comment, Please try again later";

            //ACT
            var actual = await commentServices.DeleteComment("CommentId");

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }


        private void MockUp(Comments comment, Complaint complaint, bool state)
        {
            mockComplaintRepo.Setup(service => service.GetComplaintById(It.IsAny<string>())).Returns(Task.FromResult(complaint));
            mockCommentRepo.Setup(service => service.GetCommentById(It.IsAny<string>())).Returns(Task.FromResult(comment));
            mockCommentRepo.Setup(service => service.DeleteById(It.IsAny<string>())).Returns(Task.FromResult(state));
        }
    }
}
