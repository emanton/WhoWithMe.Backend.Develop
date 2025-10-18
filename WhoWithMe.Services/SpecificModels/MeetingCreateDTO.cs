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
	}
}
