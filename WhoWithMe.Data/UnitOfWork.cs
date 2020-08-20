using Core.Data;
using Core.Data.Repositories;
using WhoWithMe.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly IContext _context;
        private Hashtable _repositories;

        public UnitOfWork(EFDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();

                if (_repositories != null)
                {
                    foreach (IDisposable repository in _repositories.Values)
                    {
                        repository.Dispose();
                    }
                }
            }

            _disposed = true;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IBaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }
            //var repositoryType = typeof(EntityRepository<>);
            //_repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
            _repositories.Add(type, new EntityRepository<TEntity>(_context));
            return (IRepository<TEntity>)_repositories[type];
        }
	}
}
