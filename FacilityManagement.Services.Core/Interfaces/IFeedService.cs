using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IFeedService
    {
        Task<Response<Pagination<ReturnedFeedDTO>>> GetFeedByPages(int pageNumber);
        Task<Response<ReturnedFeedDTO>> RetrieveFeedById(string id); 
        Task<Response<ReturnedFeedDTO>> GetFeedByName(string name);
        Task<Response<FeedReadDto>> AddFeed(FeedDTO category);
        Task<Response<string>> EditFeed(string id, FeedDTO model);
        Task<Response<string>> DeleteFeed(string id);       
    }
}
