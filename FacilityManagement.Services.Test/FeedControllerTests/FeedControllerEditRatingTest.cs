using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FacilityManagement.Services.Test
{
    class FeedControllerEditRatingTest
    {
        private IServiceProvider _serviceProvider;
        public Mock<IReplyService> mockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IComplaintService> mockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> mockFeedServices { get; set; } = new Mock<IFeedService>();
        public Mock<IRatingService> mockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> mockCommentService { get; set; } = new Mock<ICommentService>();
        public static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        public Mock<UserManager<User>> userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        [SetUp]
        public void SetUp()
        {


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
        public async Task TestDeleteRatingValid()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);
            var expected = 200;

            //ACT
            var actual = await feedController.EditRating(new EditRatingDTO(), "") as OkObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task TestDeleteRatingInValid()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);
            var expected = 400;

            //ACT
            var actual = await feedController.EditRating(new EditRatingDTO(), "") as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        private void MockUp(bool state)
        {
            mockRatingService.Setup(service => service.EditRating(It.IsAny<EditRatingDTO>(), It.IsAny<string>())).
                Returns(Task.FromResult(new Response<EditRatingDTO> { Success = state }));
        }
    }
}
