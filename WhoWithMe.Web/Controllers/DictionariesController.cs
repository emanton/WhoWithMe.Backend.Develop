using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhoWithMe.DTO.Meeting;
using WhoWithMe.Services.Interfaces;

namespace WhoWithMe.Web.Controllers
{
	public class DictionariesController : BaseController
	{
		private readonly IDictionaryService _dictionaryService;

		public DictionariesController(ILogger<MeetingController> logger, IDictionaryService dictionaryService) : base(logger)
		{
			_dictionaryService = dictionaryService;
		}

		[HttpPost("GetMeetingTypes")]
		public async Task<IActionResult> GetMeetingTypes() => await WithoutJwtWrap(_dictionaryService.GetMeetingTypes);

		[HttpPost("AddMeetingType")]
		public async Task<IActionResult> AddMeetingType(MeetingTypeDTO meetingType) => await WithoutJwtWrap(_dictionaryService.AddMeetingType, meetingType);

		[HttpPost("GetCities")]
		public async Task<IActionResult> GetCities() => await WithoutJwtWrap(_dictionaryService.GetCities);

		[HttpPost("AddCity")]
		public async Task<IActionResult> AddCity(CityDTO city) => await WithoutJwtWrap(_dictionaryService.AddCity, city);
	}
}
