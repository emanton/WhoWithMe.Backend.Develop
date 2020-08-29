using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingSubscriberDTO : CurrentUserTmp
	{
		public long UserId { get; set; }
		public long MeetingId { get; set; }
		public bool IsAccepted { get; set; }

		public MeetingSubscriber GetMeetingSubscriber()
		{
			return new MeetingSubscriber
			{
				UserId = UserId,
				MeetingId = MeetingId,
			};
		}
	}
}
