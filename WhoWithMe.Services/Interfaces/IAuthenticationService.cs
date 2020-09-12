using WhoWithMe.DTO.Model.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.DTO.Authorization;

namespace WhoWithMe.Services.Interfaces
{
	public interface IAuthenticationService
	{
		Task<UserWithToken> EmailRegister(LoginData loginData);
		Task<UserWithToken> EmailLogin(LoginData loginData);
		Task<UserWithToken> FacebookLogin(string accessToken);
	}
}
