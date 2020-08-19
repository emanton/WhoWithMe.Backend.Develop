using Autofac.Integration.WebApi;
using IoC;
using IoC.Factory;
using Owin;
using SkillsFindingService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace SkillsFindingService
{
    public class AutofacConfiguration
    {

        /// <summary>
        /// Configures the autofac.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="config">The configuration.</param>
        public static void ConfigureAutofac(IAppBuilder app, HttpConfiguration config)
        {
            var factory = new DependecyResoloverFactory();

            factory.RegisterModules(new DependencyInjectionModule());

            factory.ContainerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            DependencyResolver.ConfigureResolver(factory);

            config.DependencyResolver = DependencyResolver.Resolver;

            app.UseAutofacMiddleware(DependencyResolver.AutofacContainer);
            app.UseAutofacWebApi(config);
        }
    }
}