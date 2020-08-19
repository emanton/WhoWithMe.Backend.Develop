using Services.Interfaces;
using Services.Model.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.IdentityModel.Tokens;

namespace Services.Implementation
{
	public class AuthentificationService : IAuthentificationService
	{
		public async Task<string> EmailLogin(LoginData loginData)
		{
			LoginData user = AuthenticateUser(loginData);
			return GenerateJWT(user);
		}

		private LoginData AuthenticateUser(LoginData loginData)
		{
			if (loginData.Login == "1")
			{
				return loginData;
			}

			throw new Exception("User not found");
		}

		private string GenerateJWT(LoginData loginData)
		{
			return "";
			//string issuer = "issuer";
			//string secretKey = "EmAntonAleksandrovich1995secretKey";
			//byte[] key = Encoding.UTF8.GetBytes(secretKey);
			//SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			//var cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			//JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			//var tokenDescriptor = new SecurityTokenDescriptor
			//{
			//	Subject = new ClaimsIdentity(
			//		new Claim[]
			//		{
			//			new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, "Em") ,// maybe wrong namespace
			//			new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, loginData.Login), // maybe wrong namespace
			//			new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, new Guid().ToString()) // maybe wrong namespace
			//		}),
			//	Expires = DateTime.Now.AddDays(1),
			//	SigningCredentials = cred,
			//	Issuer = issuer,
			//	Audience = issuer
			//};
			//SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			//return tokenHandler.WriteToken(token);
		}

		public Task<bool> EmailRegister(LoginData loginData)
		{
			throw new NotImplementedException();
		}
	}
}
