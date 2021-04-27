using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IImageService
    {
        Task<UploadResult> UploadImage(IFormFile model);
    }
}
