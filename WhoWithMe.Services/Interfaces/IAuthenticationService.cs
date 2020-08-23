using WhoWithMe.DTO.Model.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoWithMe.Services.Interfaces
{
	public interface IAuthenticationService
	{
		Task<string> EmailRegister(LoginData loginData);
		Task<string> EmailLogin(LoginData loginData);
		Task<string> FacebookLogin(string accessToken);
	}
}
