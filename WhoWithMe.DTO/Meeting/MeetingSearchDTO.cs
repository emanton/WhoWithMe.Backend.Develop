using System;
using System.Collections.Generic;
using System.Text;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.DTO.Model;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingSearchDTO : Pagination
	{
		public int? MeetingTypeId { get; set; }
		//public int? MeetingSortTypeId { get; set; }
	}
}
