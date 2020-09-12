using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;

namespace WhoWithMe.DTO.Meeting.Abstract
{
	// update
    public class MeetingBaseDTO// : CurrentUserTmp
	{
		public string Title { get; set; }
		public string AvatarImageUrl { get; set; }
		public string Description { get; set; }
		public string Requirements { get; set; }
		public long CreatorId { get; set; }
		public long CityId { get; set; }
		public long MeetingTypeId { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }

		public virtual Core.Entities.Meeting GetMeeting()
		{
			return new Core.Entities.Meeting
			{
				Title = Title,
				AvatarImageUrl = AvatarImageUrl,
				Description = Description,
				Requirements = Requirements,
				CreatorId = CreatorId,
				CityId = CityId,
				MeetingTypeId = MeetingTypeId,
				Latitude = Latitude,
				Longitude = Longitude
			};
		}
	}
}
