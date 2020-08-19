using IoC.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Web.Http.Dependencies;

namespace IoC
{
    public static class DependencyResolver
    {
        private static readonly Object _setLocker = new Object();
        private static IResolverFactory _dependencyResolverFactory;
        private static volatile bool _isInited;
        private static IDependencyResolver _resolver { get; set; }

        public static void ConfigureResolver(IResolverFactory resolverFactory)
        {
            if (resolverFactory == null)
            {
                throw new NullReferenceException("Resolver factory can't be null");
            }

            if (_isInited)
            {
                throw new InvalidOperationException("DependencyResolver allready configured");
            }

            InitResolverFactory(resolverFactory);
        }

        public static IContainer AutofacContainer
        {
            get { return _dependencyResolverFactory.AutofacContainer; }
        }

        public static IDependencyResolver Resolver
        {
            get { return _resolver; }
        }

        public static TAbstraction Get<TAbstraction>() where TAbstraction : class
        {
            if (_isInited == false)
            {
                throw new InvalidOperationException("DependencyResolver is not configured");
            }

            return _resolver.GetService(typeof(TAbstraction)) as TAbstraction;
        }

        /// <summary>
        ///     Initialize resolver factory
        /// </summary>
        /// <param name="resolverFactory"></param>
        private static void InitResolverFactory(IResolverFactory resolverFactory)
        {
            lock (_setLocker)
            {
                _dependencyResolverFactory = resolverFactory;
                _dependencyResolverFactory.CreateResolver();
                _resolver = _dependencyResolverFactory.GetResolver();
                _isInited = true;
            }
        }

        public static void Dispose()
        {
            lock (_setLocker)
            {
                _isInited = false;
                _resolver = null;
                _dependencyResolverFactory = null;
            }
        }
    }
}
