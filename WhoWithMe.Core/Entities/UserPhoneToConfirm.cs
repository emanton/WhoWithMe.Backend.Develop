using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace WhoWithMe.Core.Entities
{
	public class UserPhoneToConfirm : BaseEntity
	{
		[ForeignKey("User")]
		public long UserId { get; set; }
		[JsonIgnore]
		public virtual User User { get; set; }
		public long PhoneNumber { get; set; }
		public int ConfirmPassword { get; set; }
		public DateTime DateTimeCreated { get; set; }
	}
}
