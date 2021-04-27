using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class ComplaintRepository : GenericRepository<Complaint>, IComplaintRepository
    {
        private readonly DataContext _ctx;
        private readonly DbSet<Complaint> entity;

        public ComplaintRepository(DataContext ctx) : base(ctx)
        {
            _ctx = ctx;
            entity = _ctx.Set<Complaint>();
        }

        public async Task<ICollection<Complaint>> GetComplaintsByPageNumber(int pageNumber, int per_page, string categoryId)
        {
            var allFeeds = GetAll().Where(model => model.CategoryId == categoryId).Include(model => model.User);
            var pagedItems = await GetPaginated(pageNumber, per_page, allFeeds);
            return pagedItems;
        }

        public async Task<Complaint> GetComplaintById(string complaintId)
        {
            var complaint = await _ctx.Complaints.Include(model => model.User).Include(model => model.Ratings).
                Include(model => model.Comments).ThenInclude(model => model.User).
                Include(model => model.Comments).ThenInclude(model => model.Replies).
                ThenInclude(model => model.User).FirstOrDefaultAsync(complaint => complaint.Id == complaintId);

            return complaint;
        }
    }
}