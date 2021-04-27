using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingsRepo;

        public RatingService(IServiceProvider serviceProvider)
        {
            _ratingsRepo = serviceProvider.GetRequiredService<IRatingRepository>();
        }

        public async Task<Response<string>> DeleteRating(string ratingId)
        {
            var response = new Response<string>();
            var rating = _ratingsRepo.GetById(ratingId).Result;
            if (rating != null && await _ratingsRepo.DeleteById(rating.Id))
            {
                response.Success = true;
                response.Message = "Rating deleted";
                return response;
            }
            response.Success = false;
            response.Message = "Rating not found";
            return response;
        }

        public async Task<Response<EditRatingDTO>> EditRating(EditRatingDTO model, string ratingId)
        {
            var response = new Response<EditRatingDTO>();
            var rating = _ratingsRepo.GetById(ratingId).Result;
            if (rating != null)
            {
                rating.Rating = model.Rating;
                if (await _ratingsRepo.Modify(rating))
                {
                    response.Success = true;
                    response.Data = new EditRatingDTO { Rating = rating.Rating };
                    response.Message = "Rating updated successfully";
                    return response;
                }
            }
            response.Success = false;
            response.Message = "Rating not found";
            return response;
        }

        public async Task<Response<RatingToReturnDTO>> RateComplain(string complaintId, string userId, RatingDTO ratingDTO)
        {
            Response<RatingToReturnDTO> response = new Response<RatingToReturnDTO>();
            if (await _ratingsRepo.FindUserRating(userId, complaintId) != null) 
            {
                response.Message = "sorry!, you can only rate a comment once";
                return response;
            }
            var ratingEntity = new Ratings()
            {
                Rating = ratingDTO.Rating,
                UserId = userId,
                ComplaintId = complaintId,
            };

            if (await _ratingsRepo.Add(ratingEntity))
            {
                var ratingToReturnDTO = new RatingToReturnDTO()
                {
                    RatingId = ratingEntity.Id,
                    Rating = ratingEntity.Rating,
                    UserId = ratingEntity.UserId,
                    ComplaintId = ratingEntity.ComplaintId,
                };
                response.Success = true;
                response.Data = ratingToReturnDTO;
                response.Message = "Rating added successfully";
                return response;
            }
            response.Message = "Failed to add rating";
            return response;
        }
    }
}
