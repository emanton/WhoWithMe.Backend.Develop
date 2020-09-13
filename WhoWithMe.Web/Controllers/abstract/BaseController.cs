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
using WhoWithMe.Services.Exceptions;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Web.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public abstract class BaseController : ControllerBase
	{
		private readonly ILogger _logger;

		public BaseController(ILogger logger)
		{
			_logger = logger;
		}

		protected async Task<IActionResult> WrapWithCurrentUser<MIn, T>(Func<MIn, long, Task<T>> method, MIn inParam)// where MIn : ICurrentUserTmp
		{
			
			string userIdString = User?.FindFirst("UserId")?.Value;
			long.TryParse(userIdString, out long userId);
			ResponseWrapper<T> response = new ResponseWrapper<T>();
			try
			{
				response.Data = await method.Invoke(inParam, userId);
			}
			catch (BadRequestException ex)
			{
				response.SetError(ex.Message, ex.Status);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message + ex.InnerException);
			}
			return new JsonResult(response);
		}

		protected async Task<IActionResult> Wrap<MIn, T>(Func<MIn, Task<T>> method, MIn inParam) //where MIn : ICurrentUserTmp
		{
			//string userId = User?.FindFirst("sub")?.Value;
			//inParam.CurrentUserId = userId;
			ResponseWrapper<T> response = new ResponseWrapper<T>();
			try
			{
				response.Data = await method.Invoke(inParam);
			}
			catch (BadRequestException ex)
			{
				response.SetError(ex.Message, ex.Status);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message + ex.InnerException);
			}
			return new JsonResult(response);
		}

		protected async Task<IActionResult> Wrap<MIn>(Func<MIn, Task> method, MIn inParam) where MIn : ICurrentUserTmp
		{
			string userId = User?.FindFirst("sub")?.Value;
			inParam.CurrentUserId = userId;
			ResponseWrapper response = new ResponseWrapper();
			try
			{
				await method.Invoke(inParam);
			}
			catch (BadRequestException ex)
			{
				response.SetError(ex.Message, ex.Status);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message);
			}
			return new JsonResult(response);
		}

		protected async Task<IActionResult> Wrap<MOut>(Func<Task<MOut>> method)
		{
			ResponseWrapper<MOut> response = new ResponseWrapper<MOut>();
			try
			{
				response.Data = await method.Invoke();
			}
			catch (BadRequestException ex)
			{
				response.SetError(ex.Message, ex.Status);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message + ex.InnerException);
			}
			return new JsonResult(response);
		}

		protected async Task<IActionResult> Wrap(Func<Task> method)
		{
			ResponseWrapper response = new ResponseWrapper();
			try
			{
				await method.Invoke();
			}
			catch (BadRequestException ex)
			{
				response.SetError(ex.Message, ex.Status);
			}
			catch (Exception ex)
			{
				response.SetError(ex.Message);
			}
			return new JsonResult(response);
		}

		//protected async Task<IActionResult> WithoutJwtWrap<MIn, T>(Func<MIn, Task<T>> method, MIn inParam)
		//{
		//	ResponseWrapper<T> response = new ResponseWrapper<T>();
		//	try
		//	{
		//		response.Data = await method.Invoke(inParam);
		//	}
		//	catch (BadRequestException ex)
		//	{
		//		response.SetError(ex.Message, ex.Status);
		//	}
		//	catch (Exception ex)
		//	{
		//		response.SetError(ex.Message);
		//	}
		//	return new JsonResult(response);
		//}

		//protected async Task<IActionResult> WithoutJwtWrap<MIn>(Func<MIn, Task> method, MIn inParam)
		//{
		//	ResponseWrapper response = new ResponseWrapper();
		//	try
		//	{
		//		await method.Invoke(inParam);
		//	}
		//	catch (BadRequestException ex)
		//	{
		//		response.SetError(ex.Message, ex.Status);
		//	}
		//	catch (Exception ex)
		//	{
		//		response.SetError(ex.Message);
		//	}
		//	return new JsonResult(response);
		//}

		//protected async Task<IActionResult> WithoutJwtWrap<T>(Func<Task<T>> method)
		//{
		//	ResponseWrapper<T> response = new ResponseWrapper<T>();
		//	try
		//	{
		//		response.Data = await method.Invoke();
		//	}
		//	catch (BadRequestException ex)
		//	{
		//		response.SetError(ex.Message, ex.Status);
		//	}
		//	catch (Exception ex)
		//	{
		//		response.SetError(ex.Message);
		//	}
		//	return new JsonResult(response);
		//}

		//protected async Task<IActionResult> WithoutJwtWrap(Func<Task> method)
		//{
		//	ResponseWrapper response = new ResponseWrapper();
		//	try
		//	{
		//		await method.Invoke();
		//	}
		//	catch (BadRequestException ex)
		//	{
		//		response.SetError(ex.Message, ex.Status);
		//	}
		//	catch (Exception ex)
		//	{
		//		response.SetError(ex.Message);
		//	}
		//	return new JsonResult(response);
		//}
	}
}
