using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWithMe.Services.SpecificModels
{
	public class MeetingPhoto
	{
		public long MeetingId { get; set; }
		public IFormFile FormFile { get; set; }
	}

	public class UserPhoto
	{
		public long UserId { get; set; }
		public IFormFile FormFile { get; set; }
	}
}
