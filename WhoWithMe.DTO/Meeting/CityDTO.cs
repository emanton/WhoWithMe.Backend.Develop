using System;
using System.Collections.Generic;
using System.Text;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities.dictionaries;

namespace WhoWithMe.DTO.Meeting
{
	public class CityDTO// : CurrentUserTmp
	{
		public string Name { get; set; }
		public City GetCity()
		{
			return new City
			{
				Name = Name
			};
		}
	}
}
