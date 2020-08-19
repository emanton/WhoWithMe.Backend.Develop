using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class UserSubscriber : BaseEntity
    {
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
    }
}
