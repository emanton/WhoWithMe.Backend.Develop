using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WhoWithMe.Services.Implementation;
using WhoWithMe.Services.Interfaces;
using WhoWithMe.Data;
using WhoWithMe.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace WhoWithMe.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			string securityKey = "EmAntonAleksandrovich1995secretKey";
			string issuer = "issuer";

			services.AddControllers();
			services.AddSwaggerGen();
			services.AddCors(options =>
				options.AddPolicy("CorsPolicy", builder => builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					.Build())
			);

			string connectionString = GetConnectionString();
			services.AddDbContext<EFDbContext>(options => options.UseSqlServer(connectionString));
			services.AddTransient<IContext, EFDbContext>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IUserService, UserService>();

			//services.AddScoped<IAuthenticationService, AuthenticationService>();
			//services.AddScoped<IAuthenticationService, AuthenticationService>();
			//services.AddScoped<IAuthenticationService, AuthenticationService>();
			//services.AddScoped<IAuthenticationService, AuthenticationService>();

			// auth
			services
			.AddAuthentication(auth =>
			{
				auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(jwt =>
			{
				jwt.RequireHttpsMetadata = false;
				jwt.TokenValidationParameters.ValidateIssuerSigningKey = true;
				jwt.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
				jwt.TokenValidationParameters.ValidIssuer = issuer;
				jwt.TokenValidationParameters.ValidAudience = issuer;
				jwt.TokenValidationParameters.ValidateIssuer = true;
				jwt.TokenValidationParameters.ValidateAudience = true;
			});
			//.AddFacebook(options =>
			//{
			//	options.AppId = "2735362606749096";
			//	options.AppSecret = "ab085fd41f10e4fabc13823e6b9f1d92";
			//});
			// auth

			services.AddMvc(); //added by me
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// swagger
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});
			// swagger

			

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private string GetConnectionString()
		{
			return "Server=tcp:whowithme.database.windows.net,1433;Initial Catalog = WhoWithMe; Persist Security Info=False;User ID = whowithmekim; Password=Qwerty11;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";
			//return new SqlConnectionStringBuilder
			//{
			//	//UserID = "whowithmekim@whowithme",
			//	//Password = "Qwerty11",
			//	DataSource = "tcp:whowithme.database.windows.net,1433",
			//	InitialCatalog = "WhoWithMeDBDevelop",
			//	IntegratedSecurity = true,
			//}.ConnectionString;
		}
	}
}
