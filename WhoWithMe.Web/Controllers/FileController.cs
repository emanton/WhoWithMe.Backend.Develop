using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WhoWithMe.Services.Helpers;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Services.SpecificModels;

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
		public async Task<IActionResult> Test(IFormFile userPhoto) => await WithoutJwtWrap(S3StorageService.UploadUserFile, new UserPhoto { FormFile = userPhoto });
		[HttpPost("UploadUserFile")]
		public async Task<IActionResult> UploadUserFile(UserPhoto userPhoto) => await WithoutJwtWrap(S3StorageService.UploadUserFile, userPhoto);
		[HttpPost("UploadMeetingFile")]
		public async Task<IActionResult> UploadMeetingFile(MeetingPhoto userPhoto) => await WithoutJwtWrap(S3StorageService.UploadMeetingFile, userPhoto);
	}
}
