using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Model.Meeting;
using WhoWithMe.DTO.UserDTOs;
using System.Collections.Generic;

namespace WhoWithMe.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ILogger<CommentController> logger, ICommentService commentService) : base(logger)
        {
            _commentService = commentService;
        }

        [HttpPost("GetUserComments")]
        public async Task<IActionResult> GetUserComments([FromBody] PaginationUserId pagination) => await Wrap(_commentService.GetUserComments, pagination);

        [HttpPost("AddUserComment")]
        public async Task<IActionResult> AddUserComment([FromBody] CommentUser commentUser) => await Wrap(_commentService.AddUserComments, commentUser);

        [HttpPost("DeleteUserComment")]
        public async Task<IActionResult> DeleteUserComment([FromBody] CommentUser commentUser) => await Wrap(_commentService.DeleteUserComments, commentUser);

        [HttpPost("GetMeetingComments")]
        public async Task<IActionResult> GetMeetingComments([FromBody] PaginationMeetingId pagination) => await Wrap(_commentService.GetMeetingComments, pagination);

        [HttpPost("AddMeetingComment")]
        public async Task<IActionResult> AddMeetingComment([FromBody] CommentMeeting commentMeeting) => await Wrap(_commentService.AddMeetingComments, commentMeeting);

        [HttpPost("DeleteMeetingComment")]
        public async Task<IActionResult> DeleteMeetingComment([FromBody] CommentMeeting commentMeeting) => await Wrap(_commentService.DeleteMeetingComments, commentMeeting);
    }
}
