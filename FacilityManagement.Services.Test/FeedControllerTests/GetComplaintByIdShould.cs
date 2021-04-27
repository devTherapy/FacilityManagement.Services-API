using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FacilityManagement.Services.Test
{
    class ServiceOfGetComplaintByIdShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintService> mockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> mockFeedServices { get; set; } = new Mock<IFeedService>();
        public Mock<IReplyService> mockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> mockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> mockCommentService { get; set; } = new Mock<ICommentService>();


        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(mockFeedServices.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(mockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(mockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingService))).Returns(mockRatingService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(mockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(injector => injector.GetService(typeof(UserManager<User>)))
                .Returns(userManager.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestGetComplaintsByIdValid()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.GetComplaintById("") as OkObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status200OK, actual.StatusCode);
        }

        [Test]
        public async Task TestGetComplaintsByIdInValid()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.GetComplaintById("") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            mockComplaintService.Setup(service => service.GetComplaintById(It.IsAny<string>())).
                Returns(Task.FromResult(new Response<ComplaintsDTO> { Success = state }));
        }
    }
}
