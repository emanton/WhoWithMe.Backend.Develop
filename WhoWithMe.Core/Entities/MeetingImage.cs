using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Entities.Abstract;

namespace WhoWithMe.Core.Entities
{
	public class MeetingImage : BaseImage
	{
		[ForeignKey("Meeting")]
		public long MeetingId { get; set; }
		[JsonIgnore]
		public Meeting Meeting { get; set; }
	}
}
