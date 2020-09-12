using System;
using System.Collections.Generic;
using System.Text;
using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Entities.dictionaries;

namespace WhoWithMe.DTO.UserDTOs
{
	public class UserDTO
	{
        public UserDTO(User user)
        {
            user.Id = Id;
            AvatarImageUrl = user.AvatarImageUrl;
            Nickname = user.Nickname;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            Email = user.Email;
            Phone = user.Phone;
            FacebookId = user.FacebookId;
            GmailId = user.GmailId;
            City = user.City;
        }
        public long Id { get; set; }
        public string AvatarImageUrl { get; set; }
        public string Nickname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public City City { get; set; }
        public string FacebookId { get; set; }
        public string GmailId { get; set; }
        //public virtual List<MeetingSubscriber> MeetingSubscribers { get; set; }

        public User GetUser()
		{
            User user = new User();
            user.Id = Id;
            user.AvatarImageUrl = AvatarImageUrl;
            user.Nickname = Nickname;
            user.Firstname = Firstname;
            user.Lastname = Lastname;
            user.Email = Email;
            user.Phone = Phone;
            user.FacebookId = FacebookId;
            user.GmailId = GmailId;
            user.City = City;
            return user;
        }
    }
}
