using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public class FeedMapper
    {
        public static ReturnedFeedDTO ToFeedDTO(Category category)
        {
            var result = new ReturnedFeedDTO
            {
                Id = category.Id,
                Description = category.Description,
                Name = category.Name
            };
            return result;
        }
    }
}
