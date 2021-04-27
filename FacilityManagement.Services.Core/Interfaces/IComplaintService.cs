using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IComplaintService
    {
        Task<Response<ComplaintsDTO>> GetComplaintById(string complaintId);
        Task<Response<Pagination<PaginatedComplaintsDTO>>> GetComplaintsByPage(int pageNumber, string categoryId);
        Task<Response<ComplaintResponseDTO>> AddComplaint(string feedId, AddComplaintDTO complaint);
        Task<Response<string>> EditComplaint(string complaintId, EditComplaintDTO complaint);
        Task<Response<string>> DeleteComplaint(string complaintId);

    }
}
