using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities
{
    public class MessageBase : BaseEntity
    {
		[ForeignKey("Sender")]
		public long SenderId { get; set; }
		[ForeignKey("Receiver")]
		public long ReceiverId { get; set; }
		public string Text { get; set; }
		public DateTime Created { get; set; }
		public virtual User Sender { get; set; }
		public virtual User Receiver { get; set; }
	}
}
