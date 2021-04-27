using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class FeedService : IFeedService
    {
        private readonly int PerPage = 10;
        private readonly IFeedRepository _feedRepo;

        public FeedService(IServiceProvider serviceProvider)
        {
            _feedRepo = serviceProvider.GetRequiredService<IFeedRepository>();
        }

        public async Task<Response<Pagination<ReturnedFeedDTO>>> GetFeedByPages(int pageNumber)
        {
            Response<Pagination<ReturnedFeedDTO>> response = new Response<Pagination<ReturnedFeedDTO>>();
            var feeds = await _feedRepo.GetCategoriesByPageNumber(pageNumber, PerPage);

            if (feeds != null)
            {
                response.Success = true;
                response.Message = "Feeds";
                response.Data = new Pagination<ReturnedFeedDTO>
                {
                    TotalNumberOfItems = _feedRepo.TotalNumberOfItems,
                    TotalNumberOfPages = _feedRepo.TotalNumberOfPages,
                    CurrentPage = pageNumber,
                    ItemsPerPage = PerPage,
                    Items = PaginationMappers.ForCategory(feeds)
                };
                return response;
            }
            response.Success = false;
            response.Message = "Could not feed by page number";
            return response;
        }

        public async Task<Response<ReturnedFeedDTO>> RetrieveFeedById(string id)
        {
            var response = new Response<ReturnedFeedDTO>();
            var feed = await _feedRepo.GetById(id);
            if(feed is null)
            {
                response.Message = "Invalid feed id provided";
                return response;
            }

            var data = FeedMapper.ToFeedDTO(feed);

            if (data is null)
            {
                response.Message = "Problem loading feed";
                return response;
            }

            response.Success = true;
            response.Message = "Feed details by id provided";
            response.Data = data;
            return response;
        }

        public async Task<Response<ReturnedFeedDTO>> GetFeedByName(string name)
        {
            var response = new Response<ReturnedFeedDTO>();
            var feed = await _feedRepo.GetFeedByName(name);
            if (feed is null)
            {
                response.Message = "Invalid feed id provided";
                return response;
            }

            var data = FeedMapper.ToFeedDTO(feed);

            if (data is null)
            {
                response.Message = "Problem loading feed";
                return response;
            }

            response.Success = true;
            response.Message = "Getting feed details by name";
            response.Data = data;
            return response;
        }

        public async Task<Response<FeedReadDto>> AddFeed(FeedDTO model)
        {
            var response = new Response<FeedReadDto>();
            var category = FeedProfile.FeedDtoToFeedMapper(model);
            if (await _feedRepo.Add(category))
            {
                var feedDto = FeedProfile.FeedToFeedReadDtoMapper(category);
                response.Success = true;
                response.Message = "category created successfully";
                response.Data = feedDto;
                return response;
            }

            response.Message = "category failed to add";
            return response;
        }

        public async Task<Response<string>> EditFeed(string categoryId, FeedDTO model)
        {
            var response = new Response<string>();
            var category = await _feedRepo.GetById(categoryId);
            if(category is null)
            {
                response.Message = "Invalid feed id provided";
                return response;
            }

            category.Name = model.Name;
            category.Description = model.Description;

            if (await _feedRepo.Modify(category))
            {
                response.Success = true;
                response.Message = "category edited successfully";
                return response;
            }

            response.Message = "could not update category";
            return response;
        }

        public async Task<Response<string>> DeleteFeed(string id)
        {
            var response = new Response<string>();
            var feed = await RetrieveFeedById(id);
            if(feed is null)
            {
                response.Message = "Invalid feed id provided";
                return response;
            }

            if(await _feedRepo.DeleteById(id))
            {
                response.Success = true;
                response.Message = "feed deleted successfully";
                return response;
            }

            response.Message = "could not delete feed";
            return response;
        }
    }
}
