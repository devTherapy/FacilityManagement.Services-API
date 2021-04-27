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
    public class ServiceOfAddComplaintShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintService> mockComplaintService { get; set; } = new Mock<IComplaintService>();
        public Mock<IFeedService> mockFeedService { get; set; } = new Mock<IFeedService>();
        public Mock<IReplyService> mockRepliesService { get; set; } = new Mock<IReplyService>();
        public Mock<IRatingService> mockRatingService { get; set; } = new Mock<IRatingService>();
        public Mock<ICommentService> mockCommentService { get; set; } = new Mock<ICommentService>();

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedService))).Returns(mockFeedService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintService))).Returns(mockComplaintService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentService))).Returns(mockCommentService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingService))).Returns(mockRatingService.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyService))).Returns(mockRepliesService.Object).Verifiable();
            mockServiceProvider.Setup(injector => injector.GetService(typeof(UserManager<User>)))
                .Returns(userManager.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task CreatedResponse()
        {
            //Arrange
            MockUp(true);
            var feedController = new FeedController(_serviceProvider);
            var complaintDTO = new AddComplaintDTO();
            var expected = 201;

            //ACT
            var actual = await feedController.AddComplaint("", complaintDTO) as CreatedResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        [Test]
        public async Task BadRequestResponse()
        {
            //Arrange
            MockUp(false);
            var feedController = new FeedController(_serviceProvider);
            var complaintDTO = new AddComplaintDTO();
            var expected = 400;

            //ACT
            var actual = await feedController.AddComplaint("", complaintDTO) as BadRequestObjectResult;

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual.StatusCode);
        }

        private void MockUp(bool status)
        {
            mockComplaintService.Setup(service => service.AddComplaint(It.IsAny<string>(), It.IsAny<AddComplaintDTO>())).
               Returns(Task.FromResult(new Response<ComplaintResponseDTO>() { Success = status }));
        }
    }
}