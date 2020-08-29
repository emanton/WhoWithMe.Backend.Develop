using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Exceptions
{
	public class BadRequestException : Exception
	{
		public BadRequestException(string errorMessage) : base(errorMessage)
		{

		}
		public int Status { get; set; } = 400;
	}
}
