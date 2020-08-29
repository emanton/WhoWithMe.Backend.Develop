using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.DTO.Model.Authorization;
using WhoWithMe.Web.Models;
using WhoWithMe.DTO;

namespace WhoWithMe.Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;

		public UserController(ILogger<AuthorizationController> logger, IUserService userService) : base(logger)
		{
			_userService = userService;
		}
		
		[HttpPost("GetUsers")]
		public async Task<IActionResult> GetUsers(FromToLong fromTo) => await Wrap(_userService.GetUsers, fromTo);

		[HttpPost("GetUserInfo")]
		public async Task<IActionResult> GetUserInfo(long id) => await WithoutJwtWrap(_userService.GetUserInfo, id);

		[HttpPost("DeleteUser")]
		public async Task<IActionResult> DeleteUser(long id) => await WithoutJwtWrap(_userService.DeleteUser, id);
	}
}
