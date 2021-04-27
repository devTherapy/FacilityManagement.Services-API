using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class ReplyService: IReplyService
    {
        private readonly ICommentRepository _commentsRepo;
        private readonly IReplyRepository _repliesRepo;

        public ReplyService(IServiceProvider serviceProvider)
        {
            _repliesRepo = serviceProvider.GetRequiredService<IReplyRepository>();
            _commentsRepo = serviceProvider.GetRequiredService<ICommentRepository>();
        }

        public async Task<Response<ReplyDTO>> CreateReply(string commentId, User user, AddReplyDTO dto)
        {
            var response = new Response<ReplyDTO>();
            var comment = _commentsRepo.GetById(commentId);
            if(comment is null)
            {
                response.Message = "Wrong comment id";
                return response;
            }
            var reply = new Replies
            {
                Reply = dto.Reply,
                CommentId = commentId,
                UserId = user.Id
            };

            if (await _repliesRepo.Add(reply))
            {
                var clientReply = new ReplyDTO
                {
                    Id = reply.Id,
                    Reply = reply.Reply,
                    UserId = user.Id,
                    CommentId = commentId
                };

                response.Success = true; 
                response.Message = "comment added successful";
                response.Data = clientReply;
                return response;
            }
              
            response.Message = "oops! something went wrong" ;
            return response;
        }

        public async Task<Response<string>> DeleteReply(string replyId)
        {
            Response<string> response = new Response<string>();
            var result = await _repliesRepo.GetById(replyId);
            if (result is null)
            {
                response.Message = "Invalid reply id provided";
                return response;
            }

            if (await _repliesRepo.DeleteById(replyId))
            {
                response.Success = true;
                response.Message = "successfully deleted reply";
                return response;
            }
                
            response.Message = "Something went wrong we're working on it";
            return response;
        }

        public async Task<Response<string>> EditReply(EditReplyDTO model, string replyId, User user)
        {
            Response<string> response = new Response<string>();
            var result = await _repliesRepo.GetById(replyId);

            if (result is null)
            {
                response.Message = "oops! something went wrong";
                return response;
            }

            result.Reply = model.Reply;
            result.UserId = user.Id;

            if(await _repliesRepo.Modify(result))
            {
                response.Message = "successfully edited the reply";
                response.Success = true;
                return response;
            }

            response.Message = "reply not updated";
            return response;
        }

        public async Task<Response<ReplyDTO>> GetReplyById(string id)
        {
            var userReply = await _repliesRepo.GetById(id);
            Response<ReplyDTO> response = new Response<ReplyDTO>();

            if (userReply == null)
            {
                response.Message = "oops! something went wrong";
                return response;
            }

            var clientReply = new ReplyDTO
            {
                Id = userReply.Id,
                Reply = userReply.Reply,
                UserId = userReply.UserId,
                CommentId = userReply.CommentId
            };

            response.Success = true;
            response.Data = clientReply;
            response.Message = userReply.Reply;
            return response;
        }
    }
}
