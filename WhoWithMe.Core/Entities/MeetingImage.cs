using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WhoWithMe.Core.Entities.Abstract;

namespace WhoWithMe.Core.Entities
{
	public class MeetingImage : BaseImage
	{
		[ForeignKey("Meeting")]
		public long MeetingId { get; set; }
		public Meeting Meeting { get; set; }
	}
}
