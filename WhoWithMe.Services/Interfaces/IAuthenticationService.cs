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
		Task<string> EmailLogin(LoginData loginData);
		Task<bool> EmailRegister(LoginData loginData);
	}
}
