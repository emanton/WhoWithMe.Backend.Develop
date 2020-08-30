using WhoWithMe.Core.Entities.dictionaries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WhoWithMe.Core.Data;
using System.Text.Json.Serialization;

namespace WhoWithMe.Core.Entities
{
	public class Meeting : BaseEntity
	{
        public string Title { get; set; }
        public string AvatarImageUrl { get; set; }
        public string Description { get; set; }
		public string Requirements { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		[ForeignKey("Creator")]
		public long CreatorId { get; set; }
		[ForeignKey("City")]
		public long CityId { get; set; }
		[ForeignKey("MeetingType")]
		public long MeetingTypeId { get; set; }
		[JsonIgnore]
		public virtual User Creator { get; set; }
		[JsonIgnore]
		public virtual City City { get; set; }
		[JsonIgnore]
		public virtual MeetingType MeetingType { get; set; }
		[JsonIgnore]
		public virtual List<MeetingSubscriber> MeetingSubscribers { get; set; }
		[JsonIgnore]
		public virtual List<MeetingImage> MeetingImages { get; set; }
	}
}
