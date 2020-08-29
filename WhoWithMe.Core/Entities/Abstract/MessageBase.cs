using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities
{
    public class MessageBase : BaseEntity
    {
		public virtual UserChat Chat { get; set; }
		public string Text { get; set; }
		public DateTime Created { get; set; }
	}
}
