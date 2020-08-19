using Core.Entities;
using Services.Interfaces;
using Services.Model.Meeting;
using Services.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SkillsFindingService.Controllers
{
    [RoutePrefix("Comment")]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

		[HttpPost]
		[Route("GetUserComments")]
		public async Task<IHttpActionResult> GetUserComments(PaginationUserId paginationUserId)
		{
			return await Wrap(_commentService.GetUserComments, paginationUserId);
		}

		[HttpPost]
		[Route("AddUserComments")]
		public async Task<IHttpActionResult> AddUserComments(CommentUser commentUser)
		{
			return await Wrap(_commentService.AddUserComments, commentUser);
		}

		[HttpPost]
		[Route("DeleteUserComments")]
		public async Task<IHttpActionResult> DeleteUserComments(CommentUser commentUser)
		{
			return await Wrap(_commentService.DeleteUserComments, commentUser);
		}

		[HttpPost]
		[Route("GetMeetingComments")]
		public async Task<IHttpActionResult> GetMeetingComments(PaginationMeetingId paginationMeetingId)
		{
			return await Wrap(_commentService.GetMeetingComments, paginationMeetingId);
		}

		[HttpPost]
		[Route("AddMeetingComments")]
		public async Task<IHttpActionResult> AddMeetingComments(CommentMeeting commentMeeting)
		{
			return await Wrap(_commentService.AddMeetingComments, commentMeeting);
		}

		[HttpPost]
		[Route("DeleteMeetingComments")]
		public async Task<IHttpActionResult> DeleteMeetingComments(CommentMeeting commentMeeting)
		{
			return await Wrap(_commentService.DeleteMeetingComments, commentMeeting);
		}
	}
}