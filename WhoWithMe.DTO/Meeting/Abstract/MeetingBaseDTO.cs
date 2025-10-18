using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;

namespace WhoWithMe.DTO.Meeting.Abstract
{
    public class MeetingBaseDTO
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
    }
}