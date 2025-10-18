using System.Text.Json.Serialization;
using WhoWithMe.Core.Data;
using WhoWithMe.Core.Entities.dictionaries;

namespace WhoWithMe.DTO.Meeting
{
	public class MeetingTypeDTO //: CurrentUserTmp
	{
		public string Name { get; set; }

		// Mapping to entity is handled by AutoMapper
	}
}
