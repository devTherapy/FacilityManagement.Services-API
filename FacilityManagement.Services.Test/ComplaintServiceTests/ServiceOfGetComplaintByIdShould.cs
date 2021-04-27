using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using FacilityManagement.Services.Test.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.FeedControllerTest
{
    public class GetComplaintByIdShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintRepository> mockComplaintsRepo { get; set; } = new Mock<IComplaintRepository>();
        public Mock<IFeedRepository> mockFeedRepo { get; set; } = new Mock<IFeedRepository>();


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(mockComplaintsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedRepository))).Returns(mockFeedRepo.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }
        [Test]
        public async Task Test_GetFeeds_Should_Return_Success_As_True_When_A_Valid_Object_Is_Gotten()
        {
            //Arrange
            MockUp(ModelReturnHelper.ReturnComplaint());
            var ComplaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = true;
            var expectedMessage = "Complaint";

            //ACT
            var actual = await ComplaintServices.GetComplaintById("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
            Assert.IsInstanceOf<Response<ComplaintsDTO>>(actual);
        }

        [Test]
        public async Task Test_GetFeeds_Should_Return_Success_As_False_When_A_Valid_Object_Is_Gotten()
        {
            //Arrange
            MockUp(null);
            var ComplaintServices = new ComplaintServices(_serviceProvider);
            var expectedState = false;
            var expectedMessage = "Complaint Id not found";

            //ACT
            var actual = await ComplaintServices.GetComplaintById("Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.Null(actual.Data);
            Assert.AreEqual(expectedState, actual.Success);
            Assert.AreEqual(expectedMessage, actual.Message);
        }

        private void MockUp(Complaint Complaint)
        {
            mockComplaintsRepo.Setup(service => service.GetComplaintById(It.IsAny<string>())).Returns(Task.FromResult(Complaint));

        }
    }
}
