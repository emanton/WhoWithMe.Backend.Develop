using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;

namespace WhoWithMe.DTO.Meeting.Abstract
{
	// update
    public class MeetingBaseDTO// : CurrentUserTmp
	{
		public MeetingBaseDTO() { }
		public MeetingBaseDTO(MeetingBaseDTO meetingBaseDTO)
		{
			Title = meetingBaseDTO.Title;
			Address = meetingBaseDTO.Address;
			CreatedDate = meetingBaseDTO.CreatedDate;
			StartDate = meetingBaseDTO.StartDate;
			AvatarImageUrl = meetingBaseDTO.AvatarImageUrl;
			Description = meetingBaseDTO.Description;
			Requirements = meetingBaseDTO.Requirements;
			CreatorId = meetingBaseDTO.CreatorId;
			CityId = meetingBaseDTO.CityId;
			MeetingTypeId = meetingBaseDTO.MeetingTypeId;
			Latitude = meetingBaseDTO.Latitude;
			Longitude = meetingBaseDTO.Longitude;
		}

		public string Title { get; set; }
		public string Address { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime StartDate { get; set; }
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
				Address = Address,
				CreatedDate = CreatedDate,
				StartDate = StartDate,
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


	public class MeetingBaseDTOTemp// : CurrentUserTmp
	{
		public string title { get; set; }
		public string address { get; set; }
		public DateTime createdDate { get; set; }
		public DateTime startDate { get; set; }
		public string avatarImageUrl { get; set; }
		public string description { get; set; }
		public string requirements { get; set; }
		public long creatorId { get; set; }
		public long cityId { get; set; }
		public long meetingTypeId { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }

		public virtual MeetingBaseDTO GetMeetingBaseDTO()
		{
			return new MeetingBaseDTO
			{
				Title = title,
				Address = address,
				CreatedDate = createdDate,
				StartDate = startDate,
				AvatarImageUrl = avatarImageUrl,
				Description = description,
				Requirements = requirements,
				CreatorId = creatorId,
				CityId = cityId,
				MeetingTypeId = meetingTypeId,
				Latitude = latitude,
				Longitude = longitude
			};
		}
	}
}