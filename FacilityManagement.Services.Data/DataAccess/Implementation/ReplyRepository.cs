using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class ReplyRepository: GenericRepository<Replies>, IReplyRepository
    {
        private readonly DataContext _ctx;
        public ReplyRepository(DataContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }
    }
}
