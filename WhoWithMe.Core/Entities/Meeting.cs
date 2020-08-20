using WhoWithMe.Core.Entities.dictionaries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWithMe.Core.Entities
{
    public class Meeting : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
		public User Creator { get; set; }
		public City City { get; set; }
		public Place Place { get; set; }
		public MeetingType MeetingType { get; set; }
		//public List<string> ImageUrls { get; set; }
		public List<CommentMeeting> Comments { get; set; }
	}
}
