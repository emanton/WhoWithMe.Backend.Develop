using System;
using System.Collections.Generic;
using System.Text;
using WhoWithMe.Core.Entities.Abstract;

namespace WhoWithMe.Core.Entities
{
	public class UserImage : BaseImage
	{
		public User User { get; set; }
	}
}
