using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WhoWithMe.Services.Helpers;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Services.SpecificModels;
using WhoWithMe.Web.Models;

namespace WhoWithMe.Web.Controllers
{
	public class FileController : BaseController
	{
		private readonly IMeetingService _meetingService;

		public FileController(ILogger<MeetingController> logger, IMeetingService meetingService) : base(logger)
		{
			_meetingService = meetingService;
		}

		[HttpPost("TestUploadOnlyFile")]
		public async Task<IActionResult> Test(IFormFile formFile)
		{
			return new JsonResult(formFile);
		}
		//[HttpPost("TestUploadOnlyFile")]
		//public async Task<IActionResult> Test(IFormFile UserImage) => await WithoutJwtWrap(S3StorageService.UploadUserFile, new UserImage { FormFile = UserImage });
		//[HttpPost("UploadUserFile")]
		//public async Task<IActionResult> UploadUserFile(UserImage UserImage) => await WithoutJwtWrap(S3StorageService.UploadUserFile, UserImage);
		//[HttpPost("UploadMeetingFile")]
		//public async Task<IActionResult> UploadMeetingFile(MeetingImage UserImage) => await WithoutJwtWrap(S3StorageService.UploadMeetingFile, UserImage);
	}
}
