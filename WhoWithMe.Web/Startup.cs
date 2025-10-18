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
					.WithOrigins("http://localhost:5173")
					.AllowAnyMethod()
					.AllowAnyHeader()
					.AllowCredentials()
					.Build())
			);

			string connectionString = GetConnectionString();
			services.AddDbContext<EFDbContext>(options => options.UseSqlServer(connectionString));
			services.AddTransient<IContext, EFDbContext>();
			services.AddScoped<IMeetingImageService, MeetingImageService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IMeetingService, MeetingService>();
			services.AddScoped<IMeetingSubscriberService, MeetingSubscriberService>();
			services.AddScoped<IDictionaryService, DictionaryService>();

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

			// Run migrations and seed DB in development only
			if (env.IsDevelopment())
			{
				using (var scope = app.ApplicationServices.CreateScope())
				{
					var services = scope.ServiceProvider;
					var logger = services.GetService<ILogger<Startup>>();
					try
					{
						var context = services.GetRequiredService<EFDbContext>();
						context.Database.Migrate();
						DataSeeder.SeedAsync(context).GetAwaiter().GetResult();
						logger?.LogInformation("Database migrated and seeded successfully.");
					}
					catch (Exception ex)
					{
						// Log and continue. In production prefer running migrations from deployment pipeline.
						logger?.LogError(ex, "An error occurred while migrating or seeding the database.");
					}
				}
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			// Enable CORS policy
			app.UseCors("CorsPolicy");

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

        // TODO refactor. (use connection string from enviroment variables)
        private string GetConnectionString()
        {
            //// Prefer a full connection string in configuration first
            //var configured = Configuration.GetConnectionString("DefaultConnection");
            //if (!string.IsNullOrWhiteSpace(configured))
            //{
            //    return configured;
            //}

            //// Fallback to individual settings (use user secrets or environment variables in development)
            var dataSource = Configuration["Db:DataSource"];
            var user = Configuration["Db:User"];
            var password = Configuration["Db:Password"];
            var initialCatalog = Configuration["Db:InitialCatalog"];

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                UserID = user,
                Password = password,
                InitialCatalog = initialCatalog,
            };

            return builder.ConnectionString;
        }
	}
}
