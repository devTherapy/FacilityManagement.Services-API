using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class RatingRepository: GenericRepository<Ratings>, IRatingRepository
    {
        private readonly DataContext _ctx;
        private readonly DbSet<Ratings> entity;
        public RatingRepository(DataContext ctx) : base(ctx)
        {
            _ctx = ctx;
            entity = _ctx.Set<Ratings>();
        }

        public async Task<Ratings> FindUserRating(string userId, string complaintId)
        {
           return await _ctx.Ratings.FirstOrDefaultAsync(e => e.ComplaintId == userId && e.UserId == userId);
        }
    }
}
