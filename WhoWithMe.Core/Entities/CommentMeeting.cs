using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWithMe.Core.Entities
{
    public class CommentMeeting : BaseEntity
    {
        public int Estimation { get; set; }
		public User Creator { get; set; }
		public Meeting Meeting { get; set; }

	}
}
