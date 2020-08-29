using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWithMe.Core.Entities
{
    public class UserChat : BaseEntity
	{
		public virtual User Owner { get; set; }
		public virtual List<Message> Messages { get; set; }
		public long LastMessageId { get; set; } // add constraint
		public DateTime Created { get; set; }
	}
}
