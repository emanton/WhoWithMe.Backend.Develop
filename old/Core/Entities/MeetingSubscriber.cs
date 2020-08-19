using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class MeetingSubscriber : BaseEntity
    {
        public User User { get; set; }
        public Meeting Meeting { get; set; }
    }
}
