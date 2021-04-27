using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Implementation
{
    public class CommentRepository : GenericRepository<Comments>, ICommentRepository
    {
        private readonly DataContext _ctx;
        public CommentRepository(DataContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<Comments> GetCommentById(string commentId)
        {
            var comment = _ctx.Comments.Where(x => x.Id == commentId);
            if (comment.Count() > 0)
            {
                var commentWithReplies = await comment.Include(model => model.User).Include(model => model.Replies).ThenInclude(model => model.User).ToListAsync();
                return commentWithReplies.First();
            }
            return null;
        }
    }
}
