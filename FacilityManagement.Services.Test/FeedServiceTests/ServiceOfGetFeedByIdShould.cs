using Moq;
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using FacilityManagement.Services.Models;
using FacilityManagement.Services.Core.Implementation;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using Microsoft.Extensions.Configuration;

namespace FacilityManagement.Services.Test
{
    public class ServiceOfGetFeedByIdShould
    {
        private IServiceProvider _serviceProvider;
        public Mock<ICommentRepository> mockCommentsRepo { get; set; } = new Mock<ICommentRepository>();
        public Mock<IComplaintRepository> mockComplaintsRepo { get; set; } = new Mock<IComplaintRepository>();
        public Mock<IFeedRepository> mockFeedRepo { get; set; } = new Mock<IFeedRepository>();
        public Mock<IReplyRepository> mockRepliesRepo { get; set; } = new Mock<IReplyRepository>();
        //public Mock<IConfiguration> mockConfiguration { get; set; } = new Mock<IConfiguration>();
        
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
        public async Task ReturnValidFeed()
        {
            //Arrange
            MockUp(new Category());
            var feedServices = new FeedService(_serviceProvider);

            //Act
            var actual = await feedServices.RetrieveFeedById("");

            //Assert
            Assert.IsNotNull(actual.Data);
        }

        private void MockUp(Category model)
        {
            mockFeedRepo.Setup(service => service.GetById(It.IsAny<string>())).
                Returns(Task.FromResult(model));
        }
    }
}
