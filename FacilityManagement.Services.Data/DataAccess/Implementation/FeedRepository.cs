using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class FeedRepository :  GenericRepository<Category>, IFeedRepository
    {
        private readonly DataContext _ctx;
        public FeedRepository(DataContext ctx): base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Category> GetFeedByName(string name)
        {
            return await _ctx.Categories.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<ICollection<Category>> GetCategoriesByPageNumber(int pageNumber, int per_page)
        {
            var allFeeds = GetAll();
            var pagedItems = await GetPaginated(pageNumber, per_page, allFeeds);
            return pagedItems;

        }
    }
}
