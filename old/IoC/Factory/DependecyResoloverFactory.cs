using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Autofac.Integration.WebApi;

namespace IoC.Factory
{
    public class DependecyResoloverFactory : IResolverFactory
    {
        private IDependencyResolver _resolver;

        public IContainer AutofacContainer { get; set; }
        public ContainerBuilder ContainerBuilder { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public DependecyResoloverFactory()
        {
            ContainerBuilder = new ContainerBuilder();
        }

        public void RegisterModules(Module module)
        {
            ContainerBuilder.RegisterModule(module);
        }

        /// <summary>
        ///     Configure and create dependency resolver
        /// </summary>
        public void CreateResolver()
        {
            AutofacContainer = ContainerBuilder.Build();
            _resolver = new AutofacWebApiDependencyResolver(AutofacContainer);
        }

        /// <summary>
        ///     Get dependency resolver instance
        /// </summary>
        /// <returns></returns>
        public IDependencyResolver GetResolver()
        {
            return _resolver;
        }
    }
}
