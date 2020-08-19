using Services.Model.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
	public interface IAuthentificationService
	{
		Task<string> EmailLogin(LoginData loginData);
		Task<bool> EmailRegister(LoginData loginData);
	}
}
