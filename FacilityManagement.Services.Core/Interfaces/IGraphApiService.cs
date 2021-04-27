using FacilityManagement.Services.DTOs;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IGraphApiService
    {
        Task<GraphApiResponseDTO> GetAuthorizedUserDetails(string token);
    }
}
