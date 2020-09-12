using System.Collections.Generic;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Meeting.Abstract;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingEditDTO : MeetingCreateDTO, IBaseEntity
	{
		public MeetingEditDTO()
		{

		}
		public MeetingEditDTO(MeetingBaseDTO baseDto) : base(baseDto)
		{
		}
		public long Id { get; set; }
		public List<long> RemovedImageIds { get; set; }

		public override Core.Entities.Meeting GetMeeting()
		{
			Core.Entities.Meeting meeting = base.GetMeeting();
			meeting.Id = Id;
			return meeting;
		}
	}

	public class MeetingEditDTOTemp : MeetingCreateDTOTemp
	{
		public long id { get; set; }
		public List<long> removedImageIds { get; set; }

		public MeetingEditDTO GetMeetingEditDTO()
		{
			MeetingBaseDTO dto = GetMeetingBaseDTO();
			MeetingEditDTO meetingCreateDTO = new MeetingEditDTO(dto);
			meetingCreateDTO.MeetingImages = meetingImages;
			meetingCreateDTO.Id = id;
			meetingCreateDTO.RemovedImageIds = removedImageIds;
			return meetingCreateDTO;
		}
	}
}
