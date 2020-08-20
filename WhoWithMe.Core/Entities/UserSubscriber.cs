using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhoWithMe.Core.Entities
{
    public class UserSubscriber : BaseEntity
    {
		public long SubscribedUserId { get; set; } // add constraint
		public long TargetUserId { get; set; } // add constraint
	}
}
