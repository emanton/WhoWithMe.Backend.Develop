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
using WhoWithMe.Web.Middleware;
using AutoMapper;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace WhoWithMe.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string securityKey = Configuration["Jwt:Key"] ?? "EmAntonAleksandrovich1995secretKey";
            string issuer = Configuration["Jwt:Issuer"] ?? "issuer";

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
            services.AddScoped<IContext>(sp => sp.GetRequiredService<EFDbContext>());
            services.AddScoped(typeof(global::Core.Data.Repositories.IRepository<>), typeof(global::WhoWithMe.Data.Repositories.EntityRepository<>));
            services.AddScoped<IMeetingImageService, MeetingImageService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<IMeetingSubscriberService, MeetingSubscriberService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddAutoMapper(typeof(WhoWithMe.Services.Mapping.AutoMapperProfiles));

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(WhoWithMe.Services.Mapping.AutoMapperProfiles).Assembly);

            services.AddHealthChecks();

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

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

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
                        logger?.LogError(ex, "An error occurred while migrating or seeding the database.");
                    }
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private string GetConnectionString()
        {
            var configured = Configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured;
            }

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
