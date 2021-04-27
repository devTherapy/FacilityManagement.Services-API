using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface ICommentRepository : IGenericRepository<Comments>
    {
        Task<Comments> GetCommentById(string commentId);

    }
}
