using WhoWithMe.Core.Entities.dictionaries;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWithMe.Core.Entities
{
    public class User : BaseEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
		public City City { get; set; }
		public List<Meeting> CreatedMeetings { get; set; }
		//public List<string> ImageUrls { get; set; }
		public List<CommentUser> Comments { get; set; }
	}
}
