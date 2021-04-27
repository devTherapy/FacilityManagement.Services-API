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
    public class ServiceOfEditComplaintShould
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
            MockUp(new Complaint());
            var complaintServices = new ComplaintServices(_serviceProvider);
            var complaintId = "";
            var complaint = new EditComplaintDTO();

            //Act
            var actual = await complaintServices.EditComplaint(complaintId, complaint);

            //Assert
            Assert.IsTrue(actual.Success);
        }

        [Test]
        public async Task FailWhenComplaintNotFound()
        {
            //Arrange
            MockUp(null);
            var complaintServices = new ComplaintServices(_serviceProvider);
            var complaintId = "";
            var complaint = new EditComplaintDTO();

            //Act
            var actual = await complaintServices.EditComplaint(complaintId, complaint);

            //Assert
            Assert.IsFalse(actual.Success);
        }

        private void MockUp(Complaint complaint)
        {
            mockComplaintsRepo.Setup(service => service.GetById(It.IsAny<string>()))
                .Returns(Task.FromResult(complaint));
            mockComplaintsRepo.Setup(service => service.Modify(complaint))
                .Returns(Task.FromResult((complaint != null)));
        }
    }
}
