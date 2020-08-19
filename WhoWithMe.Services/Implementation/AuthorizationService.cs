using WhoWithMe.Services.Interfaces;
using WhoWithMe.DTO.Model.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Core.Entities;
using System.Text.Unicode;
using WhoWithMe.Core.Data;
//using Microsoft.IdentityModel.Tokens;

namespace WhoWithMe.Services.Implementation
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AuthenticationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<string> EmailLogin(LoginData loginData)
		{
			User user = await AuthenticateUser(loginData);
			return GenerateJWT(user);
		}

		private async Task<User> AuthenticateUser(LoginData loginData)
		{
			return await GetUserByLoginData(loginData);
		}

		private async Task<User> GetUserByLoginData(LoginData loginData)
		{
			string encodedPassword = EncodePassword(loginData.Password);
			return await _unitOfWork.Repository<User>().GetSingleAsync(x => x.Email == loginData.Email && x.Password == encodedPassword);
		}

		private string GenerateJWT(User user)
		{
			string issuer = "issuer";
			string secretKey = "EmAntonAleksandrovich1995secretKey";
			byte[] key = Encoding.UTF8.GetBytes(secretKey);
			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
			SigningCredentials cred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(
					new Claim[]
					{
						new Claim(JwtRegisteredClaimNames.Sub, "Em"),
						new Claim(JwtRegisteredClaimNames.Email, user.Email),
						new Claim(JwtRegisteredClaimNames.Jti, new Guid().ToString())
					}),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = cred,
				Issuer = issuer,
				Audience = issuer
			};
			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		private string EncodePassword(string password)
		{
			return Encoding.UTF8.GetBytes(password).ToString();
		}

		public async Task<bool> EmailRegister(LoginData loginData)
		{
			User user = new User();
			user.Email = loginData.Email;
			user.Password = EncodePassword(loginData.Password);
			_unitOfWork.Repository<User>().Insert(user);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}
	}
}
