using FacilityManagement.Services.API.Controllers;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FacilityManagement.Services.Test
{
    public class PostCommentShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintService> mockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> mockFeedServices { get; set; } = new Mock<IFeedService>();
        public Mock<ICommentService> mockCommentService { get; set; } = new Mock<ICommentService>();
        public Mock<IReplyService> mockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> mockRatingsService { get; set; } = new Mock<IRatingService>();
        private static Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
        private Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(mockFeedServices.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(mockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(mockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(mockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(service => service.GetService(typeof(IRatingService)))
                .Returns(mockRatingsService.Object);
            mockServiceProvider.Setup(service => service.GetService(typeof(UserManager<User>)))
                .Returns(mockUserManager.Object);
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestPostCommentsIsNotValid()
        {
            //Arrange
            MockUp(new Response<string>(), false);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.Comment("", new CommentDto(){Comment = "Yes, the lights are bad" }) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Test]
        public async Task TestPostCommentsIsValid()
        {
            //Arrange
            MockUp(new Response<string>(), true);
            var feedController = new FeedController(_serviceProvider);

            //ACT
            var actual = await feedController.Comment("", new CommentDto() { Comment = "Yes, the lights are bas"}) as ObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(StatusCodes.Status201Created, actual.StatusCode);
        }

        private void MockUp(Response<string> model, bool state)
        {
            mockCommentService.Setup(service => service.PostComment("", It.IsAny<User>(), It.IsAny<CommentDto>())).
               Returns(Task.FromResult(new Response<string>{Success = state}));
        }
    }
}