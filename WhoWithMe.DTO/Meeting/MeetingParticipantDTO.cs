using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingParticipantDTO : CurrentUserTmp
	{
		public long UserId { get; set; }
		public long MeetingId { get; set; }

		//public MeetingParticipant GetMeetingParticipant()
		//{
		//	return new MeetingParticipant
		//	{
		//		UserId = UserId,
		//		MeetingId = MeetingId
		//	};
		//}
	}
}
