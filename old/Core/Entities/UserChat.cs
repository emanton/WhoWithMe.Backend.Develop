using System;

namespace Core.Entities
{
    public class UserChat : BaseEntity
    {
        public User Owner { get; set; }
        public Message LastMessage { get; set; }
        public DateTime Created { get; set; }
    }
}
