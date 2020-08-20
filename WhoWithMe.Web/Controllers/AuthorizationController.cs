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
	public class AuthorizationController : ControllerBase
	{
		private readonly ILogger<AuthorizationController> _logger;
		private readonly IAuthenticationService _authorizationService;

		public AuthorizationController(ILogger<AuthorizationController> logger, IAuthenticationService authorizationService)
		{
			_logger = logger;
			_authorizationService = authorizationService;
		}
		
		[HttpGet("EmailRegister")]
		public async Task<IActionResult> EmailRegister(LoginData loginData) => await Wrap(_authorizationService.EmailRegister, loginData);

		[HttpGet("EmailLogin")]
		public async Task<IActionResult> EmailLogin(LoginData loginData) => await Wrap(_authorizationService.EmailLogin, loginData);

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

		
		//[HttpPost("GmailLogin")]
		//public async Task<IActionResult> GmailLogin(string authToken)
		//	=> await Wrap(_magicService.AddPersonalStoryToUser, personalStory);

		//[HttpPost("FacebookLogin")]
		//public async Task<IActionResult> FacebookLogin(string authToken)
		//	=>  {return "Not implemented";}


		private async Task<IActionResult> Wrap<MIn, T>(Func<MIn, Task<T>> method, MIn inParam)
		{
			ResponseWrapper<T> response = new ResponseWrapper<T>();
			try
			{
				response.Data = await method.Invoke(inParam);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message);
			}
			return new JsonResult(response);
		}

		private async Task<IActionResult> Wrap<MIn>(Func<MIn, Task> method, MIn inParam)
		{
			ResponseWrapper response = new ResponseWrapper();
			try
			{
				await method.Invoke(inParam);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message);
			}
			return new JsonResult(response);
		}
	}
}
