using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.Core.Implementation
{
    public class CommentService: ICommentService
    {
        private readonly ICommentRepository _commentsRepository;
        private readonly IComplaintRepository _complaintsRepository;

        public CommentService(IServiceProvider serviceProvider)
        {
            _complaintsRepository = serviceProvider.GetRequiredService<IComplaintRepository>();
            _commentsRepository = serviceProvider.GetRequiredService<ICommentRepository>();
        }

        public async Task<Response<Comments>> EditCommentById(EditCommentDTO dto, string id, User user)
        {
            Response<Comments> response = new Response<Comments>();
            var result = await GetCommentById(id);

            if (!result.Success)
            {
                response.Message = "Could not edit comment";
                response.Data = result.Data;
            }

            result.Data.Comment = dto.Comment;
            result.Data.Updated_at = DateTime.Now;
            result.Data.UserId = user.Id;

            if(await _commentsRepository.Modify(result.Data))
            {
                response.Message = "Successfully edited comment";
                response.Success = true;
                return response;
            }

            response.Message = "Something went wrong";
            return response;
        }

        public async Task<Response<Comments>> GetCommentById(string id)
        {
            Response<Comments> response = new Response<Comments>();
            var userComment = await _commentsRepository.GetById(id);

            if (userComment == null)
            {
                response.Message = "comment does not exist";
            }

            response.Success = true;
            response.Data = userComment;
            response.Message = userComment.Comment;
            return response;
        }

        public async Task<Response<string>> PostComment(string complaintId, User user, CommentDto comments)
        {
            Response<string> response = new Response<string>();
            var complaint = await _complaintsRepository.GetComplaintById(complaintId);
             
            if (complaint is null)
            {
                response.Message = "Complaint Not Found!";
                return response;
            }

            complaint.Comments.Add(new Comments() { User = user, Comment = comments.Comment });

            if (await _complaintsRepository.Modify(complaint))
            {
                response.Message = "Comments added!";
                response.Success = true;
                return response;
            }

            response.Message = "Comments already exists";
            return response;
        }

        public async Task<Response<string>> DeleteComment(string commentId)
        {
            var response = new Response<string>();
            var comment = await _commentsRepository.GetCommentById(commentId);
            if (comment == null)
            {
                response.Message = "Invalid comment Id";
                return response;
            }
            if (await _commentsRepository.DeleteById(commentId))
            {
                response.Success = true;
                response.Message = "Comment deleted successfully";
                return response;
            }

            response.Message = "Unable to delete comment, Please try again later";
            return response;
        }
    }
}
