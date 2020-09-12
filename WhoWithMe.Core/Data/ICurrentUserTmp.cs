using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace WhoWithMe.Core.Data
{
	public interface ICurrentUserTmp
	{

		// TODO REMOVE DELETE
		string CurrentUserId { get; set; }
	}

	//public abstract class CurrentUserTmp : ICurrentUserTmp
	//{

	//	// TODO REMOVE DELETE
	//	[NotMapped]
	//	[JsonIgnore]
	//	public string CurrentUserId { get; set; }
	//}
}
