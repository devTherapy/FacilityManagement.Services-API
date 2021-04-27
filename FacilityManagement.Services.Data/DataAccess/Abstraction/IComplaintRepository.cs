using FacilityManagement.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Data.DataAccess.Abstraction
{
    public interface IComplaintRepository : IGenericRepository<Complaint>
    {
        Task<ICollection<Complaint>> GetComplaintsByPageNumber(int pageNumber, int per_page, string categoryId);

        Task<Complaint> GetComplaintById(string complaintId);
    }
}
