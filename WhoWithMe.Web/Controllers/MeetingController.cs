using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.DTO.Model.Authorization;
using WhoWithMe.Web.Models;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.Model.User;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Model.Meeting;

namespace WhoWithMe.Web.Controllers
{
	//[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class MeetingController : BaseController
	{
		private readonly IMeetingService _meetingService;

		public MeetingController(ILogger<MeetingController> logger, IMeetingService meetingService) : base(logger)
		{
			_meetingService = meetingService;
		}
		
		[HttpPost("GetMeetingsByTitleAndType")]
		public async Task<IActionResult> GetMeetingsByTitleAndType(MeetingSearchDTO meetingSearchDTO) => await Wrap(_meetingService.GetMeetingsByTypeAndTitleAndSortType, meetingSearchDTO);

		[HttpPost("GetMeetingsByOwner")]
		public async Task<IActionResult> GetMeetingsByOwner(PaginationUserId paginationUserId) => await Wrap(_meetingService.GetMeetingsByOwner, paginationUserId);

		[HttpPost("GetMeeting")]
		public async Task<IActionResult> GetMeeting(CurrentUserIdMeetingId meetingId) => await Wrap(_meetingService.GetMeeting, meetingId);

		[HttpPost("AddMeeting")]
		public async Task<IActionResult> AddMeeting(MeetingCreateDTO meeting) => await Wrap(_meetingService.AddMeeting, meeting);

		[HttpPost("EditMeeting")]
		public async Task<IActionResult> EditMeeting(MeetingEditDTO meeting) => await Wrap(_meetingService.EditMeeting, meeting);

		[HttpPost("DeleteMeeting")]
		public async Task<IActionResult> DeleteMeeting(CurrentUserIdMeetingId meetingId) => await Wrap(_meetingService.DeleteMeeting, meetingId);
	}
}
