using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using FacilityManagement.Services.Test.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Test
{
    class ServiceOfGetComplaintsShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<IComplaintRepository> mockComplaintsRepo { get; set; } = new Mock<IComplaintRepository>();
        private ICollection<Complaint> all = new List<Complaint>();
        private Mock<IFeedRepository> mockFeedRepo { get; set; } = new Mock<IFeedRepository>();


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(mockComplaintsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedRepository))).Returns(mockFeedRepo.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }
        [Test]
        public async Task TestGetComplaintsValid()
        {
            //Arrange
            MockUp( ModelReturnHelper.ReturnComplaints(), 2);
            var ComplaintServices = new ComplaintServices(_serviceProvider);
            var expectedPageCount = 2;
            var expectedItemCount = 1;
            var expectedCurrentPage = 1;
            var expectedPerPage = 10;

            //ACT
            var actual = await ComplaintServices.GetComplaintsByPage(1, "Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Data.Items);
            Assert.True(actual.Success);
            Assert.AreEqual(expectedItemCount, actual.Data.TotalNumberOfItems);
            Assert.AreEqual(expectedPageCount, actual.Data.TotalNumberOfPages);
            Assert.AreEqual(expectedCurrentPage, actual.Data.CurrentPage);
            Assert.AreEqual(expectedPerPage, actual.Data.ItemsPerPage);
        }

        [Test]
        public async Task TestGetComplaintsInValid()
        {
            //Arrange
            MockUp2(null, 2);
            var ComplaintServices = new ComplaintServices(_serviceProvider);

            //ACT
            var actual = await ComplaintServices.GetComplaintsByPage(5, "Id");

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNull(actual.Data);
            Assert.False(actual.Success);
        }

        private void MockUp(ICollection<Complaint> allComplaints, int totalPages)
        {
            var complaintsAsQuery = allComplaints!=null? allComplaints.AsQueryable(): null;
            all = allComplaints;
            mockComplaintsRepo.Setup(service => service.GetPaginated(It.IsAny<int>(),It.IsAny<int>(),complaintsAsQuery)).Returns(Task.FromResult(all));
            mockComplaintsRepo.Setup(property => property.TotalNumberOfPages).Returns(totalPages);
            mockComplaintsRepo.Setup(service => service.GetComplaintsByPageNumber(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(allComplaints));
            mockComplaintsRepo.Setup(property => property.TotalNumberOfItems).Returns(allComplaints.Count);

        }
        private void MockUp2(ICollection<Complaint> allComplaints, int totalPages)
        {
            var complaintsAsQuery = allComplaints!=null? allComplaints.AsQueryable(): null;
            all = allComplaints;
            mockComplaintsRepo.Setup(service => service.GetPaginated(It.IsAny<int>(),It.IsAny<int>(),complaintsAsQuery)).Returns(Task.FromResult(all));
            mockComplaintsRepo.Setup(property => property.TotalNumberOfPages).Returns(totalPages);
            mockComplaintsRepo.Setup(service => service.GetComplaintsByPageNumber(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(allComplaints));

        }
    }
}
