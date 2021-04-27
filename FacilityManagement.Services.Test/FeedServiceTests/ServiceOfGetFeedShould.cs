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
    public class ServiceOfGetFeedShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<ICommentRepository> mockCommentsRepo { get; set; } = new Mock<ICommentRepository>();
        public Mock<IComplaintRepository> mockComplaintsRepo { get; set; } = new Mock<IComplaintRepository>();
        public Mock<IFeedRepository> mockFeedRepo { get; set; } = new Mock<IFeedRepository>();
        public Mock<IReplyRepository> mockRepliesRepo { get; set; } = new Mock<IReplyRepository>();
        private ICollection<Category> all = new List<Category>();


        [SetUp]
        public void SetUp()
        {
            var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IFeedRepository))).Returns(mockFeedRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IComplaintRepository))).Returns(mockComplaintsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(ICommentRepository))).Returns(mockCommentsRepo.Object).Verifiable();
            mockServiceProvider.Setup(provide => provide.GetService(typeof(IReplyRepository))).Returns(mockRepliesRepo.Object).Verifiable();

            _serviceProvider = mockServiceProvider.Object;
        }
        [Test]
        public async Task TestGetFeedsValid()
        {
            //Arrange
            MockUp(ModelReturnHelper.ReturnCategories(), 2);
            var feedServices = new FeedService(_serviceProvider);
            var expectedPageCount = 2;
            var expectedItemCount = 2;
            var expectedCurrentPage = 1;
            var expectedPerPage = 10;

            //ACT
            var actual = await feedServices.GetFeedByPages(1);

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
        public async Task TestGetFeedsInValid()
        {
            //Arrange
            MockUp2(null, 2);
            var feedServices = new FeedService(_serviceProvider);

            //ACT
            var actual = await feedServices.GetFeedByPages(5);

            //Assert
            Assert.IsNotNull(actual);
            Assert.False(actual.Success);
        }

        private void MockUp(ICollection<Category> allCategories, int totalPages)
        {
            var complaintsAsQuery = allCategories!=null? allCategories.AsQueryable(): null;
            all = allCategories;
            mockFeedRepo.Setup(service => service.GetPaginated(1,10, complaintsAsQuery)).Returns(Task.FromResult(all));
            mockFeedRepo.Setup(property => property.TotalNumberOfPages).Returns(totalPages);
            mockFeedRepo.Setup(service => service.GetCategoriesByPageNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(allCategories));
            mockFeedRepo.Setup(property => property.TotalNumberOfItems).Returns(allCategories.Count);
        }
        private void MockUp2(ICollection<Category> allCategories, int totalPages)
        {
            var complaintsAsQuery = allCategories!=null? allCategories.AsQueryable(): null;
            all = allCategories;
            mockFeedRepo.Setup(service => service.GetPaginated(1,10, complaintsAsQuery)).Returns(Task.FromResult(all));
            mockFeedRepo.Setup(property => property.TotalNumberOfPages).Returns(totalPages);
            mockFeedRepo.Setup(service => service.GetCategoriesByPageNumber(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.FromResult(allCategories));
        }
    }
}
