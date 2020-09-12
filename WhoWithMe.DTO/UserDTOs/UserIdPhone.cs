using System;
using System.Collections.Generic;
using System.Text;

namespace WhoWithMe.DTO.UserDTOs
{
	public class UserIdPhone
	{
		public long UserId { get; set; }
		public long PhoneNumber { get; set; }
	}

	public class PhoneConfirmPassword : UserIdPhone
	{
		public int ConfirmPassword { get; set; }
	}
}
