using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using Services.Interfaces;
using Services.Model.Mappings;
using SkillsFindingService;

[assembly: OwinStartup(typeof(SkillsFindingService.Startup))]

namespace SkillsFindingService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            HttpConfiguration config = new HttpConfiguration();
            //config.EnableCors();
            AutoMapperConfig.Register();
            AutofacConfiguration.ConfigureAutofac(app, config);
            WebApiConfig.Register(config);
            //SwaggerConfig.Register();

            AuthConfig.ConfigureOAuth(app);


            app.UseWebApi(config);

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
