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
using WhoWithMe.DTO.Authorization;
using WhoWithMe.Data.Repositories;
using WhoWithMe.Data;

namespace WhoWithMe.Services.Implementation
{
	// Forgot Password
	public class AuthenticationService : IAuthenticationService
	{
		private readonly IContext _context;

		public AuthenticationService(IContext context)
		{
			_context = context;
		}

		public async Task<UserWithToken> EmailRegister(LoginData loginData)
		{
			ValidatePassword(loginData.Password);
			await ValidateEmail(loginData.Email);
			User user = new User();
			user.Email = loginData.Email;
			user.Password = EncodePassword(loginData.Password);
			new EntityRepository<User>(_context).Insert(user);
			await _context.SaveChangesAsync();
			user.Nickname = "user" + user.Id;
			new EntityRepository<User>(_context).Update(user);
			await _context.SaveChangesAsync();
			return await EmailLogin(loginData);
		}

		public async Task<UserWithToken> EmailLogin(LoginData loginData)
		{
			User user = await AuthenticateUser(loginData);
			if (user == null)
			{
				throw new BadRequestException("Login or password are incorrect!");
			}

			return GetLoginResponse(user);
		}

		public async Task<UserWithToken> FacebookLogin(string accessToken)
		{
			FacebookUserInfoResult fbRes = await FacebookAuthorization.ValidateAccessTokenAsync(accessToken);
			User user = await new EntityRepository<User>(_context).GetSingleAsync(x => x.FacebookId == fbRes.Id);
			if (user == null)
			{
				User newUser = await FacebookRegister(fbRes);
				user = newUser;
			}

			return GetLoginResponse(user);
		}

		private UserWithToken GetLoginResponse(User user)
		{
			UserWithToken userWithToken = new UserWithToken(user);
			userWithToken.Token = GenerateJWT(user);
			return userWithToken;
		}

		private async Task<User> FacebookRegister(FacebookUserInfoResult fbRes)
		{
			User user = new User();
			user.FacebookId = fbRes.Id;
			user.Email = fbRes.Email;
			user.Lastname = fbRes.LastName;
			user.Firstname = fbRes.FirstName;
			user.AvatarImageUrl = fbRes.Picture?.Data.Url;
			new EntityRepository<User>(_context).Insert(user);
			await _context.SaveChangesAsync();
			user.Nickname = "user" + user.Id;
			new EntityRepository<User>(_context).Update(user);
			await _context.SaveChangesAsync();
			return user;
		}

		private async Task<User> AuthenticateUser(LoginData loginData)
		{
			return await GetUserByLoginData(loginData);
		}

		private async Task<User> GetUserByLoginData(LoginData loginData)
		{
			string encodedPassword = EncodePassword(loginData.Password);
			return await new EntityRepository<User>(_context).GetSingleAsync(x => x.Email == loginData.Email && x.Password == encodedPassword);
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
						new Claim("UserId", user.Id.ToString()),
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

			var user = await new EntityRepository<User>(_context).GetSingleAsync(x => x.Email == email);
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
