using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Interfaces
{
    public interface IReplyService
    {
        Task<Response<string>> DeleteReply(string replyId);
        Task<Response<ReplyDTO>> CreateReply(string commentId, User user, AddReplyDTO dto); 
        Task<Response<ReplyDTO>> GetReplyById(string id);
        Task<Response<string>> EditReply(EditReplyDTO model, string replyId, User user);
    }
}
