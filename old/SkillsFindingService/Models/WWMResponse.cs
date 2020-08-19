using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsFindingService.Models
{
	public class WWMResponse
	{
		public bool IsSuccess = true;
		public string ErrorMessage { get; set; }
		public void SetError(Exception ex)
		{
			IsSuccess = false;
			ErrorMessage = ex.ToString();
		}
	}

	public class WWMResponse<T> : WWMResponse
	{
		public T Data { get; set; }
	}
}