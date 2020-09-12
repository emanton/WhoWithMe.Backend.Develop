using System.Collections.Generic;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;


namespace WhoWithMe.DTO.Meeting
{
	public class MeetingEditDTO : MeetingCreateDTO, IBaseEntity
	{
		public long Id { get; set; }
		public List<long> RemovedImageIds { get; set; }

		public override Core.Entities.Meeting GetMeeting()
		{
			Core.Entities.Meeting meeting = base.GetMeeting();
			meeting.Id = Id;
			return meeting;
		}
	}
}
