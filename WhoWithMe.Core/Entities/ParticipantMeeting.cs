using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities
{
    public class ParticipantMeeting : BaseEntity
    {
		public User User { get; set; }
		public Meeting Meeting { get; set; }
	}
}
