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

namespace WhoWithMe.Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthorizationController : BaseController
	{
		private readonly IAuthenticationService _authorizationService;

		public AuthorizationController(ILogger<AuthorizationController> logger, IAuthenticationService authorizationService) : base(logger)
		{
			_authorizationService = authorizationService;
		}
		
		[HttpPost("EmailRegister")]
		public async Task<IActionResult> EmailRegister([FromBody] LoginData loginData) => await Wrap(_authorizationService.EmailRegister, loginData);

		[HttpPost("EmailLogin")]
		public async Task<IActionResult> EmailLogin([FromBody] LoginData loginData) => await Wrap(_authorizationService.EmailLogin, loginData);

		[HttpPost("FacebookLogin")]
		public async Task<IActionResult> FacebookLogin([FromBody] string accessToken) => await Wrap(_authorizationService.FacebookLogin, accessToken);

		[Authorize]
		[HttpGet("GetAuthorizedUserMessage")]
		public async Task<string> GetAuthorizedUserMessage()
		{
			return "Good job!";
		}

		[HttpGet("GetAnyUserMessage")]
		public async Task<string> GetAnyUserMessage()
		{
			return "Good job!";
		}
	}
}
