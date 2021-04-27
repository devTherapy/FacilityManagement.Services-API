using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface IRatingRepository: IGenericRepository<Ratings>
    {
        Task<Ratings> FindUserRating(string userId, string complaintId);

    }
}
