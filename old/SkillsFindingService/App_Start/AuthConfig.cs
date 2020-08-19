using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Services.Interfaces;
using SkillsFindingService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillsFindingService
{
    public class AuthConfig
    {
        public static void ConfigureOAuth(IAppBuilder app)
        {
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //For OAuth Authentication
            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/authentication/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(6),
                Provider = new SimpleAuthorizationServerProvider()
            };

            //Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}