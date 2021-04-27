using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IRatingService
    {
        Task<Response<string>> DeleteRating(string ratingId);
        Task<Response<EditRatingDTO>> EditRating(EditRatingDTO model, string ratingId); 
        Task<Response<RatingToReturnDTO>> RateComplain(string complaintId, string userId, RatingDTO ratingDTO);
    }
}
