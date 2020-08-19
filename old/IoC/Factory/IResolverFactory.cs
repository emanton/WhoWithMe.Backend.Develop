using System.Web.Http.Dependencies;
using Autofac;

namespace IoC.Factory
{
    public interface IResolverFactory
    {
        IContainer AutofacContainer { get; set; }
        void CreateResolver();
        IDependencyResolver GetResolver();
    }
}
