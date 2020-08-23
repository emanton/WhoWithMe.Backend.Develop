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
using WhoWithMe.Services.Helpers;
using WhoWithMe.Services.Exceptions;

namespace WhoWithMe.Services.Implementation
{
	// Forgot Password
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AuthenticationService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<string> EmailRegister(LoginData loginData)
		{
			ValidatePassword(loginData.Password);
			await ValidateEmail(loginData.Email);
			User user = new User();
			user.Email = loginData.Email;
			user.Nickname = loginData.Email;
			user.Password = EncodePassword(loginData.Password);
			_unitOfWork.GetRepository<User>().Insert(user);
			await _unitOfWork.SaveChangesAsync();
			return await EmailLogin(loginData);		
		}

		public async Task<string> EmailLogin(LoginData loginData)
		{
			User user = await AuthenticateUser(loginData);
			if (user == null)
			{
				throw new BadRequestException("Login or password are incorrect!");
			}
			return GenerateJWT(user);
		}

		public async Task<string> FacebookLogin(string accessToken)
		{
			FacebookUserInfoResult fbRes = await FacebookAuthorization.ValidateAccessTokenAsync(accessToken);
			User user = await _unitOfWork.GetRepository<User>().GetSingleAsync(x => x.FacebookId == fbRes.Id);
			if (user == null)
			{
				User newUser = await FacebookRegister(fbRes);
				user = newUser;
			}

			return GenerateJWT(user);
		}

		private async Task<User> FacebookRegister(FacebookUserInfoResult fbRes)
		{
			User user = new User();
			user.FacebookId = fbRes.Id;
			user.Nickname = fbRes.Id;
			user.Email = fbRes.Email;
			user.Lastname = fbRes.LastName;
			user.Firstname = fbRes.FirstName;
			user.AvatarImageUrl = fbRes.Picture?.Data.Url;
			_unitOfWork.GetRepository<User>().Insert(user);
			await _unitOfWork.SaveChangesAsync();
			return user;
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
						new Claim(JwtRegisteredClaimNames.Email, user.Id.ToString()),
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
			byte[] data = Encoding.ASCII.GetBytes(password);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			string hash = Encoding.ASCII.GetString(data);
			return hash;
		}

		private async Task ValidateEmail(string email)
		{
			bool isValid;
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				isValid = addr.Address == email;
			}
			catch
			{
				isValid = false;
			}

			if (!isValid)
			{
				throw new BadRequestException("not valid email");
			}

			var user = await _unitOfWork.GetRepository<User>().GetSingleAsync(x => x.Email == email);
			if (user != null)
			{
				throw new BadRequestException("email has already existed");
			}
		}

		private bool ValidatePassword(string password)
		{
			if (password.Length < 5)
			{
				throw new BadRequestException("Password needs to be > 5");
			}
			if (password.Length > 20)
			{
				throw new BadRequestException("Password needs to be < 20");
			}

			return true;
		}
	}
}
