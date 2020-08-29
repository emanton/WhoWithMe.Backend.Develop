using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingView : Core.Entities.Meeting
	{
		public int ParticipantsCount { get; set; }
		public int SubscribersCount { get; set; }
		public int CommentsCount { get; set; }

		public MeetingView()
		{

		}
		public MeetingView(Core.Entities.Meeting meeting)
		{
			AvatarImageUrl = meeting.AvatarImageUrl;
			CityId = meeting.CityId;
			CreatorId = meeting.CreatorId;
			Description = meeting.Description;
			Id = meeting.Id;
			Latitude = meeting.Latitude;
			Longitude = meeting.Longitude;
			MeetingTypeId = meeting.MeetingTypeId;
			Requirements = meeting.Requirements;
			Title = meeting.Title;
		}
	}
}
