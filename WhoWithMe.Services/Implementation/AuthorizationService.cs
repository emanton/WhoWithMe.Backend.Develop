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

		public async Task<bool> EmailRegister(LoginData loginData)
		{
			if (!IsValidEmail(loginData.Email))
			{
				throw new Exception("Email is invalid");
			}
			if (!IsValidPassword(loginData.Password))
			{
				throw new Exception("Password is invalid");
			}

			User user = new User();
			user.Email = loginData.Email;
			user.Password = EncodePassword(loginData.Password);
			_unitOfWork.GetRepository<User>().Insert(user);
			await _unitOfWork.SaveChangesAsync();
			return true;
		}

		public async Task<string> EmailLogin(LoginData loginData)
		{
			User user = await AuthenticateUser(loginData);
			if (user == null)
			{
				throw new Exception("Login or password are incorrect!");
			}
			return GenerateJWT(user);
		}

		private async Task<User> AuthenticateUser(LoginData loginData)
		{
			return await GetUserByLoginData(loginData);
		}

		private async Task<User> GetUserByLoginData(LoginData loginData)
		{
			string encodedPassword = EncodePassword(loginData.Password);
			return await _unitOfWork.GetRepository<User>().GetSingleAsync(x => x.Email == loginData.Email && x.Password == encodedPassword);
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
			byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			String hash = System.Text.Encoding.ASCII.GetString(data);
			return hash;
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
		private bool IsValidPassword(string password)
		{
			if (password.Length < 5)
			{
				throw new Exception("Password needs to be > 5");
			}
			if (password.Length > 20)
			{
				throw new Exception("Password needs to be < 20");
			}

			return true;
		}
	}
}
