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
    public class FeedControllerCrudTest
    {
        private IServiceProvider _serviceProvider;
        public Mock<IFeedService> MockFeedService { get; set; } = new Mock<IFeedService>();
        public Mock<IReplyService> mockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> mockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> mockCommentService { get; set; } = new Mock<ICommentService>();
        public Mock<IComplaintService> mockComplaintService { get; set; } = new Mock<IComplaintService>();


        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(MockFeedService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(mockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(mockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingService))).Returns(mockRatingService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(mockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(injector => injector.GetService(typeof(UserManager<User>)))
                .Returns(userManager.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task ShouldReturnOkResultWhenValidModelPassed()
        {
            //Arrange
            var model = new FeedDTO();
            MockFeedService.Setup(x => x.AddFeed(It.IsAny<FeedDTO>()))
                .Returns(Task.FromResult(new Response<FeedReadDto>() { Success = true }));
            var feedController = new FeedController(_serviceProvider);

            var expected = 201;

            //ACT
            var actual = await feedController.CreateFeed(model) as CreatedResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
        [Test]
        public async Task ShouldReturnBadRequestWhenInValidModelPassed()
        {
            //Arrange
            var model = new FeedDTO();
            MockFeedService.Setup(x => x.AddFeed(It.IsAny<FeedDTO>()))
                .Returns(Task.FromResult(new Response<FeedReadDto>() { Success = false }));
            var feedController = new FeedController(_serviceProvider);
            

            var expected = 400;

            //ACT
            var actual = await feedController.CreateFeed(model) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
        [Test]
        public async Task ShouldReturnBadRequestWhenIdNotValid()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            MockFeedService.Setup(x => x.DeleteFeed(It.IsAny<string>()))
                .Returns(Task.FromResult(new Response<string>() { Success = false }));
            var feedController = new FeedController(_serviceProvider);

            var expected = 400;

            //Act
            var actual = await feedController.RemoveFeed(id) as BadRequestObjectResult;

            //Assert
           // Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
        [Test]
        public async Task ShouldReturnOkResultWhenIdValid()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            MockFeedService.Setup(x => x.DeleteFeed(It.IsAny<string>()))
                .Returns(Task.FromResult(new Response<string>() { Success = true }));
            var feedController = new FeedController(_serviceProvider);

            var expected = 200;

            //Act
            var actual = await feedController.RemoveFeed(id) as OkObjectResult;

            //Assert
            // Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
        [Test]
        public async Task ShouldReturnBadRequestIfEditNotSuccessful()
        {
            //Arrange
            var model = new FeedDTO();
            var id = Guid.NewGuid().ToString();
            MockFeedService.Setup(x => x.EditFeed(It.IsAny<string>(),It.IsAny<FeedDTO>()))
                .Returns(Task.FromResult(new Response<string>() { Success = false }));
            var feedController = new FeedController(_serviceProvider);

            var expected = 400;

            //Act
            var actual = await feedController.ModifyFeed(id,model) as BadRequestObjectResult;

            //Assert
            // Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }
        [Test]
        public async Task ShouldReturnNoContentIfEditSuccessful()
        {
            //Arrange
            var model = new FeedDTO();
            var id = Guid.NewGuid().ToString();
            MockFeedService.Setup(x => x.EditFeed(It.IsAny<string>(), It.IsAny<FeedDTO>()))
                .Returns(Task.FromResult(new Response<string>() { Success = true }));
            var feedController = new FeedController(_serviceProvider);

            var expected = 204;

            //Act
            var actual = await feedController.ModifyFeed(id, model) as NoContentResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

    }
}
