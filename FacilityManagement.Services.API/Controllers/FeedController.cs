using FacilityManagement.Services.API.Policy;
using FacilityManagement.Services.Core.Interfaces;
using FacilityManagement.Services.DTOs;
using FacilityManagement.Services.DTOs.ManualMappers;
using FacilityManagement.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FacilityManagement.Services.API.Controllers
{
    /// <summary>
    /// FeedController class
    /// </summary>
    public class FeedController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IFeedService _feedServices;
        private readonly IComplaintService _complaintsServices;
        private readonly IReplyService _repliesServices;
        private readonly IRatingService _ratingsServices;
        private readonly ICommentService _commentsService;

        /// <summary>
        /// Feed Controller constructor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public FeedController(IServiceProvider serviceProvider)
        {
            _feedServices = serviceProvider.GetRequiredService<IFeedService>();
            _complaintsServices = serviceProvider.GetRequiredService<IComplaintService>();
            _repliesServices = serviceProvider.GetRequiredService<IReplyService>();
            _ratingsServices = serviceProvider.GetRequiredService<IRatingService>();
            _commentsService = serviceProvider.GetRequiredService<ICommentService>();
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        }

        /// <summary>
        /// Gets fields by pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("get-feeds/{pageNumber}")]
        [Authorize]
        public async Task<IActionResult> GetFeeds(int pageNumber)
        {
            var result = await _feedServices.GetFeedByPages(pageNumber);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Returns a feed category by id
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        [HttpGet("get-feed/{feedId}")]
        [Authorize]
        public async Task<IActionResult> GetFeedById([FromRoute] string feedId)
        {
            var result = await _feedServices.RetrieveFeedById(feedId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Returns a feed category by id
        /// </summary>
        /// <param name="feedName"></param>
        /// <returns></returns>
        [HttpGet("get-feed-by-name/{feedName}")]
        public async Task<IActionResult> GetFeedByName([FromRoute] string feedName)
        {
            var result = await _feedServices.GetFeedByName(feedName);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// adds a feed 
        /// </summary>
        /// <returns></returns>
        [HttpPost("add-feed")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> CreateFeed(FeedDTO model)
        {
            var result = await _feedServices.AddFeed(model);
            if (result.Success) return Created("", result);
            return BadRequest(result);
        }

        /// <summary>
        /// modifies a category
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("edit-feed/{feedId}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> ModifyFeed([FromRoute] string feedId, [FromBody] FeedDTO model)
        {
            var result = await _feedServices.EditFeed(feedId, model);
            if (result.Success) return NoContent();
            return BadRequest(result);
        }

        ///<summary>
        /// deletes a category by Id
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        [HttpDelete("delete-feed/{feedId}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> RemoveFeed([FromRoute] string feedId)
        {
            var result = await _feedServices.DeleteFeed(feedId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Gets complaints by pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="feedId"></param>
        /// <returns></returns>
        [HttpGet("{feedId}/get-complaints/{pageNumber}")]
        public async Task<IActionResult> GetComplaintsByPage([FromRoute] int pageNumber, [FromRoute] string feedId)
        {
            var result = await _complaintsServices.GetComplaintsByPage(pageNumber, feedId);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Gets Complaints by Complaint Id
        /// </summary>
        /// <param name="complaintId"></param>
        /// <returns></returns>
        [HttpGet("get-complaint/{complaintId}", Name = "GetComplaintById")]
        public async Task<IActionResult> GetComplaintById([FromRoute] string complaintId)
        {
            var result = await _complaintsServices.GetComplaintById(complaintId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// route for adding a new complaint to the database
        /// </summary>
        /// <param name="feedId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("{feedId}/add-complaint")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> AddComplaint([FromRoute] string feedId, [FromBody] AddComplaintDTO model)
        {
            var result = await _complaintsServices.AddComplaint(feedId, model);
            if (result.Success) return Created("", result);
            return BadRequest(result);
        }

        ///<summary>
        /// route for editing an existing complaint in database
        /// </summary>
        /// <param name="complaintId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("edit-complaint/{complaintId}")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> EditComplaint([FromRoute] string complaintId, [FromBody] EditComplaintDTO model)
        {
            var result = await _complaintsServices.EditComplaint(complaintId, model);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Deletes a complaint 
        /// </summary>
        /// <param name="complaintId"></param>
        /// <returns></returns>
        [HttpDelete("delete-complaint/{complaintId}")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> DeleteComplaint(string complaintId)
        {
            var response = await _complaintsServices.DeleteComplaint(complaintId);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Adds rating to a comment
        /// </summary>
        /// <param name="complaintId"></param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost("complaint/{complaintId}/add-rate")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> RateComplaint(string complaintId, RatingDTO rating)
        {
            var user = await _userManager.GetUserAsync(User);
            var response = await _ratingsServices.RateComplain(complaintId, user.Id, rating);
            if (response.Success) return Created("", response);
            return BadRequest(response);
        }

        /// <summary>
        /// Edits a user's rating on a complaint
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        [HttpPatch("complaint/edit-rate/{ratingId}")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> EditRating([FromBody] EditRatingDTO rating, [FromRoute] string ratingId)
        {
            var result = await _ratingsServices.EditRating(rating, ratingId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Deletes a user's rating on a complaint
        /// </summary>
        /// <param name="ratingId"></param>
        /// <returns></returns>
        [HttpDelete("complaint/delete-rate/{ratingId}")]
        [Authorize(Policy = Policies.Decadev)]
        public async Task<IActionResult> DeleteRating(string ratingId)
        {
            var result = await _ratingsServices.DeleteRating(ratingId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        /// <summary>
        /// Adds A comment to an existing Complaint and Associates it to a User
        /// </summary>
        /// <param name="complaintId"></param>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        [HttpPost("complaint/{complaintId}/add-comment")]
        [AuthorizeMultiplePolicy(new string[] { Policies.Admin, Policies.FacilityManager, Policies.Vendor })]
        public async Task<IActionResult> Comment(string complaintId, [FromBody] CommentDto commentModel)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var result = await _commentsService.PostComment(complaintId, appUser, commentModel);
            return !result.Success ? BadRequest(result) : StatusCode(201, result);
        }

        /// <summary>
        /// Edits a user's comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("complaint/edit-comment/{commentId}")]
        [AuthorizeMultiplePolicy(new string[] { Policies.Admin, Policies.FacilityManager, Policies.Vendor })]
        public async Task<IActionResult> EditComment([FromRoute] string commentId, [FromBody] EditCommentDTO model)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var comment = await _commentsService.EditCommentById(model, commentId, loggedInUser);
            if (!comment.Success) return BadRequest(comment);
            return NoContent();
        }
       
        /// <summary>
        /// Deletes a comment
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        [HttpDelete("complaint/delete-comment/{commentId}")]
        [AuthorizeMultiplePolicy(new string[] { Policies.Admin, Policies.FacilityManager, Policies.Vendor })]
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            var response = await _commentsService.DeleteComment(commentId);
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        /// <summary>
        /// Add a User's Reply to Database
        /// </summary>
        /// /// <param name="commentId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("comment/{commentId}/add-reply")]
        [Authorize]
        public async Task<IActionResult> AddReply(string commentId, [FromBody] AddReplyDTO model)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var result = await _repliesServices.CreateReply(commentId, loggedInUser, model);
            if (!result.Success) return BadRequest(result);
            return Created("", result);
        }

        /// <summary>
        /// Edits a user's a reply
        /// </summary>
        /// <param name="replyId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("edit-reply/{replyId}")]
        [Authorize]
        public async Task<IActionResult> EditReply([FromRoute] string replyId, [FromBody] EditReplyDTO model)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var result = await _repliesServices.EditReply(model, replyId, loggedInUser);
            if (!result.Success) return BadRequest(result);
            return NoContent();
        }

        /// <summary>
        /// Deletes a reply from a comment
        /// </summary>
        /// <param name="replyId"></param>
        /// <returns></returns>
        [HttpDelete("delete-reply/{replyId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReply([FromRoute] string replyId)
        {
            var result = await _repliesServices.DeleteReply(replyId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}