using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities
{
    public class CommentUser : BaseEntity
    {
        public int Estimation { get; set; }
		public long UserToId { get; set; } // add constraint
		public long UserFromId { get; set; } // add constraint
	}
}
