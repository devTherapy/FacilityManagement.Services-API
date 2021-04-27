using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test.ComplaintServiceTests
{
    class ServiceOfAddComplaintShould
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
        public async Task ReturnValidResponse()
        {
            //Arrange
            MockUp(new Category());
            var complaintServices = new ComplaintServices(_serviceProvider);
            var feedId = "";
            var complaint = new AddComplaintDTO();

            //Act
            var actual = await complaintServices.AddComplaint(feedId, complaint);

            //Assert
            Assert.IsNotNull(actual.Data);
        }

        [Test]
        public async Task FailWhenCategoryNotFound()
        {
            //Arrange
            MockUp(null);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var feedId = "";
            var complaint = new AddComplaintDTO();

            //Act
            var actual = await complaintServices.AddComplaint(feedId, complaint);

            //Assert
            Assert.IsNull(actual.Data);
        }

        private void MockUp(Category feed)
        {
            mockComplaintsRepo.Setup(service => service.Add(It.IsAny<Complaint>()))
                .Returns(Task.FromResult(true));
            mockFeedRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(feed));
            mockFeedRepo.Setup(services => services.Modify(It.IsAny<Category>()))
                .Returns(Task.FromResult(true));
        }
    }
}
