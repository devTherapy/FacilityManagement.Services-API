using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.Data.DataAccess.Abstraction;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.Core.Implementation
{
    public class ComplaintServices : IComplaintService
    {
        private readonly IComplaintRepository _complaintsRepo;
        private readonly int PerPage = 10;
        private readonly IFeedRepository _feedRepo;

        public ComplaintServices(IServiceProvider serviceProvider)
        {
            _complaintsRepo = serviceProvider.GetRequiredService<IComplaintRepository>();
            _feedRepo = serviceProvider.GetRequiredService<IFeedRepository>();
        }

        public async Task<Response<Pagination<PaginatedComplaintsDTO>>> GetComplaintsByPage(int pageNumber, string categoryId)
        {
            Response<Pagination<PaginatedComplaintsDTO>> response = new Response<Pagination<PaginatedComplaintsDTO>>();
            var complaints = await _complaintsRepo.GetComplaintsByPageNumber(pageNumber, PerPage, categoryId);
            if (complaints != null)
            {
                response.Success = true;
                response.Message = "Complaints paginated";
                response.Data = new Pagination<PaginatedComplaintsDTO>
                {
                    TotalNumberOfItems = _complaintsRepo.TotalNumberOfItems,
                    TotalNumberOfPages = _complaintsRepo.TotalNumberOfPages,
                    CurrentPage = pageNumber,
                    ItemsPerPage = PerPage,
                    Items = PaginationMappers.ForPaginatedComplaints(complaints)
                };
                return response;
            }
            response.Message = "Something went wrong";
            return response;
        }

        public async Task<Response<ComplaintsDTO>> GetComplaintById(string complaintId)
        {
            Response<ComplaintsDTO> response = new Response<ComplaintsDTO>();
            var complaint = await _complaintsRepo.GetComplaintById(complaintId);
            if (complaint == null)
            {
                response.Message = "Complaint Id not found";
                return response;
            }

            response.Success = true;
            response.Message = "Complaint";
            response.Data = PaginationMappers.ForComplaints(complaint);
            return response;
        }

        /// <summary>
        /// Adds a complaint to an existing feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="complaint"></param>
        /// <returns></returns>
        public async Task<Response<ComplaintResponseDTO>> AddComplaint(string feedId, AddComplaintDTO complaint)
        {
            var response = new Response<ComplaintResponseDTO>();
            var feed = await _feedRepo.GetById(feedId);
            //ends the process if the feed cannot be found
            if (feed == null)
            {
                response.Message = "Feed not found";
                response.Success = false;
                return response;
            }

            //map values of the DTO to the actual object
            var newComplaint = ComplaintMapper.FromAddComplaintDTO(complaint);

            //updates the feedId in the categoryId property
            newComplaint.CategoryId = feedId;
            var result = await _complaintsRepo.Add(newComplaint);

            response.Success = result;

            response.Message = !response.Success ? "Error occured while updating your entry" : "Updated successfully";
            response.Data = ComplaintMapper.ToComplaintResponseDTO(newComplaint);

            //returns the complaint added and the related field
            return response;
        }

        /// <summary>
        /// Updates a complaint in the database
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="complaintId"></param>
        /// <param name="complaint"></param>
        /// <returns></returns>
        public async Task<Response<string>> EditComplaint(string complaintId, EditComplaintDTO complaint)
        {
            var response = new Response<string>();

            var complaintToEdit = await _complaintsRepo.GetById(complaintId);
            //end the process if the complaint is not found
            if (complaintToEdit == null)
            {
                response.Message = "Error retrieving complaint";
                return response;
            }

            //updates dynamic values of the complaint
            complaintToEdit.Question = complaint.Question;
            complaintToEdit.Image = complaint.Image;
            complaintToEdit.Title = complaint.Title;
            complaintToEdit.IsTask = complaint.IsTask;

            //updates the status of the entity class
            await _complaintsRepo.Modify(complaintToEdit);

            response.Message = "Updated successfully";
            response.Success = true;
            return response;
        }

        public async Task<Response<string>> DeleteComplaint(string complaintId)
        {
            Response<string> response = new Response<string>();
            var complaint = await _complaintsRepo.GetComplaintById(complaintId);
            if (complaint == null)
            {
                response.Message = "Invalid complaint Id";
                return response;
            }
            if (await _complaintsRepo.DeleteById(complaintId))
            {
                response.Success = true;
                response.Message = "Complaint deleted successfully";
                return response;
            }
            response.Message = "Unable to delete complaint, Plese try again later";
            return response;
        }
    }
}
