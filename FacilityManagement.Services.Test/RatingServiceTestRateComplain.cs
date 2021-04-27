using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    class RatingServiceTestRateComplain
    {
        private IServiceProvider _serviceProvider;
        public Mock<IRatingRepository> mockRatingsRepo { get; set; } = new Mock<IRatingRepository>();
        public Mock<IComplaintRepository> mockComplaintRepo { get; set; } = new Mock<IComplaintRepository>();

        public Mock<ICommentRepository> mockCommentsRepo { get; set; } = new Mock<ICommentRepository>();

        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IRatingRepository))).Returns(mockRatingsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentRepository))).Returns(mockCommentsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(mockComplaintRepo.Object).Verifiable();
            _serviceProvider = mockServiceProvider.Object;
        }

        [Test]
        public async Task TestRateComplaintValid()
        {
            //Arrange
            MockUp(new Complaint(), null, true);
            var ratingServices = new RatingService(_serviceProvider);
            var expectedState = true;
            var ratingDTO = new RatingDTO();
            var expectedMessage = "Rating added successfully";

            //ACT
            var actual = await ratingServices.RateComplain("complaintId", "userId", ratingDTO);

            //Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }


        [Test]
        public async Task TestRateComplaintInvalid()
        {
            //Arrange
            MockUp(new Complaint(), new Ratings(), false);
            var ratingServices = new RatingService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "sorry!, you can only rate a comment once";
            var ratingDTO = new RatingDTO();

            //ACT
            var actual = await ratingServices.RateComplain("complaintId", "userId", ratingDTO);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        public async Task TestRateComplaintInvalid2()
        {
            //Arrange
            MockUp(new Complaint(), new Ratings(), false);
            var ratingServices = new RatingService(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Failed to add rating";
            var ratingDTO = new RatingDTO();

            //ACT
            var actual = await ratingServices.RateComplain("complaintId", "userId", ratingDTO);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }
        private void MockUp(Complaint complain, Ratings rating, bool state)
        {
            mockComplaintRepo.Setup(service => service.GetComplaintById(It.IsAny<string>())).Returns(Task.FromResult(complain));
            mockRatingsRepo.Setup(service => service.DeleteById(It.IsAny<string>())).Returns(Task.FromResult(state));
            mockRatingsRepo.Setup(service => service.Add(It.IsAny<Ratings>())).Returns(Task.FromResult(state));
            mockRatingsRepo.Setup(service => service.FindUserRating(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(rating));

        }
    }
}
