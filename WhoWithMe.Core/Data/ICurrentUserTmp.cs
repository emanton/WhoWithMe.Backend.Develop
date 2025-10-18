using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace WhoWithMe.Core.Data
{
	public interface ICurrentUserTmp
	{
		string CurrentUserId { get; set; }
	}
}
