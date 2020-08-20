using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWithMe.Core.Entities
{
    public class UserChat : BaseEntity
	{
		public User Owner { get; set; }
		public List<Message> Messages { get; set; }
		public long LastMessageId { get; set; } // add constraint
		public DateTime Created { get; set; }
	}
}
