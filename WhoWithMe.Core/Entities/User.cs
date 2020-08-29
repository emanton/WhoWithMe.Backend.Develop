using WhoWithMe.Core.Entities.dictionaries;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhoWithMe.Core.Entities
{
    public class User : BaseEntity
    {
        public string AvatarImageUrl { get; set; }
        public string Nickname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        [JsonIgnore]
        public virtual City City { get; set; }
        public string FacebookId { get; set; }
        public string GmailId { get; set; }
        [JsonIgnore]
        public virtual List<MeetingSubscriber> MeetingSubscribers { get; set; }
    }
}
