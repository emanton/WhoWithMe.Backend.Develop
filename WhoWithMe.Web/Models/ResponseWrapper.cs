using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoWithMe.Web.Models
{
	public class ResponseWrapper
	{
		public void SetError(string errorMessage, int statusCode = 500)
		{
			ErrorMessage = errorMessage;
			StatusCode = statusCode;
		}
		public int StatusCode { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class ResponseWrapper<T> : ResponseWrapper
	{
		public T Data { get; set; }
	}
}
