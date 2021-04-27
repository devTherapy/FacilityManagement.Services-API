using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public static class FeedProfile
    {
        public static Category FeedDtoToFeedMapper(FeedDTO model)
        {
            return new Category
            {
                Name = model.Name,
                Description = model.Description
            };
        }
        public static FeedReadDto FeedToFeedReadDtoMapper(Category category)
        {
            return new FeedReadDto()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
