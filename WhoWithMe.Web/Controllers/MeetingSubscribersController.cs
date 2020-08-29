using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.DTO.Model.Meeting;
using WhoWithMe.Services.Interfaces;

namespace WhoWithMe.Web.Controllers
{
	public class MeetingSubscribersController : BaseController
	{
		private readonly IMeetingSubscriberService _meetingSubscriberService;

		public MeetingSubscribersController(ILogger<MeetingController> logger, IMeetingSubscriberService meetingSubscriberService) : base(logger)
		{
			_meetingSubscriberService = meetingSubscriberService;
		}

		[HttpPost("GetMeetingSubscribers")]
		public async Task<IActionResult> GetMeetingSubscribers(PaginationMeetingId paginationMeetingId) => await Wrap(_meetingSubscriberService.GetMeetingNotAcceptedSubscribers, paginationMeetingId);

		[HttpPost("AddMeetingSubscriber")]
		public async Task<IActionResult> AddMeetingSubscriber(MeetingSubscriberDTO meetingSubscriber) => await Wrap(_meetingSubscriberService.AddMeetingSubscriber, meetingSubscriber);

		[HttpPost("DeleteMeetingSubscriber")]
		public async Task<IActionResult> DeleteMeetingSubscriber(UserMeetingId participantMeetingId) => await Wrap(_meetingSubscriberService.DeleteMeetingSubscriber, participantMeetingId);
	}
}
