using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities.Abstract
{
	public abstract class MeetingUserBase : BaseEntity
	{
		[ForeignKey("User")]
		public long UserId { get; set; }
		[ForeignKey("Meeting")]
		public long MeetingId { get; set; }
		public User User { get; set; }
		public Meeting Meeting { get; set; }
	}
}
