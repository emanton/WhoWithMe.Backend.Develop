using System;
using System.Collections.Generic;
using System.Text;
using WhoWithMe.Core.Entities;
using WhoWithMe.DTO.UserDTOs;

namespace WhoWithMe.DTO.Authorization
{
	public class UserWithToken : UserDTO
	{
		public UserWithToken(User user) : base(user) { }
		public string Token { get; set; }
	}
}
