using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface ICommentService
    {
        Task<Response<string>> PostComment(string complaintId, User user, CommentDto comments);
        Task<Response<string>> DeleteComment(string commentId);
        Task<Response<Comments>> GetCommentById(string id);
        Task<Response<Comments>> EditCommentById(EditCommentDTO dto, string id, User user);
    }
}