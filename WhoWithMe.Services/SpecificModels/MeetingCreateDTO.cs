using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Meeting.Abstract;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingCreateDTO : MeetingBaseDTO
	{
		public MeetingCreateDTO()
		{
		}
		public MeetingCreateDTO(MeetingBaseDTO baseDto) : base(baseDto)
		{
		}
		public IEnumerable<FormFile> MeetingImages { get; set; }
	}

	public class MeetingCreateDTOTemp : MeetingBaseDTOTemp
	{
		public IEnumerable<FormFile> meetingImages { get; set; }
		public MeetingCreateDTO GetMeetingCreateDTO()
		{
			MeetingBaseDTO dto = GetMeetingBaseDTO();
			MeetingCreateDTO meetingCreateDTO = new MeetingCreateDTO(dto);
			meetingCreateDTO.MeetingImages = meetingImages;
			return meetingCreateDTO;
		}
	}
	
}
