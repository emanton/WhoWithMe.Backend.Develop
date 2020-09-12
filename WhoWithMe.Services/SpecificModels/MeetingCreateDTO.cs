using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.Meeting.Abstract;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingCreateDTO : MeetingBaseDTO
	{
		public IEnumerable<IFormFile> MeetingImages { get; set; }
	}
}
