using WhoWithMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WhoWithMe.Core.Entities
{
    public class CommentMeeting : BaseEntity
    {
        public int Estimation { get; set; }

        [Required]
        [MinLength(1)]
        public string Text { get; set; } // comment body

		public virtual User Creator { get; set; }
		public virtual Meeting Meeting { get; set; }
    }
}
