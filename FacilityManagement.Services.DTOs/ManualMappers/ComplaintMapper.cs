using FacilityManagement.Services.Models;

namespace FacilityManagement.Services.DTOs.ManualMappers
{
    public class ComplaintMapper
    {
        public static ComplaintResponseDTO ToComplaintResponseDTO(Complaint complaint)
        {
            return new ComplaintResponseDTO
            {
                Id = complaint.Id,
                Type = complaint.Type,
                Image = complaint.Image,
                IsTask = complaint.IsTask,
                CategoryId = complaint.CategoryId,
                UserId = complaint.UserId,
                Comments = complaint.Comments,
                Question = complaint.Question,
                Ratings = complaint.Ratings,
                Title = complaint.Title
            };
        }

        public static Complaint FromAddComplaintDTO(AddComplaintDTO complaint)
        {
            return new Complaint
            {
                Type = complaint.Type,
                Image = complaint.Image,
                IsTask = complaint.IsTask,
                Question = complaint.Question,
                UserId = complaint.UserId
            };
        }

        public static Complaint FromEditComplaintDTO(EditComplaintDTO complaint)
        {
            return new Complaint
            {
                Type = complaint.Type,
                Image = complaint.Image,
                IsTask = complaint.IsTask,
                Question = complaint.Question
            };
        }
    }
}
