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
	public class MeetingParticipantsController : BaseController
	{
		private readonly IMeetingSubscriberService _meetingSubscriberService;

		public MeetingParticipantsController(ILogger<MeetingController> logger, IMeetingSubscriberService meetingSubscriberService) : base(logger)
		{
			_meetingSubscriberService = meetingSubscriberService;
		}

		[HttpPost("GetMeetingParticipants")]
		public async Task<IActionResult> GetMeetingParticipants(PaginationMeetingId paginationMeetingId) => await Wrap(_meetingSubscriberService.GetMeetingAcceptedSubscribers, paginationMeetingId);

		[HttpPost("UpdateMeetingSubscriberAcceptStatus")]
		public async Task<IActionResult> AcceptMeetingSubscriber(MeetingSubscriberDTO meetingSubscriber) => await Wrap(_meetingSubscriberService.UpdateMeetingSubscriberAcceptStatus, meetingSubscriber);
	}
}
