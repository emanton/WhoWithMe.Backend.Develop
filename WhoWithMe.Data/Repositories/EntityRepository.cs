using Core.Data;
using Core.Data.Repositories;
using Core.Data.Repositories.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Data.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly IContext _context;
        private readonly DbSet<TEntity> _dbEntitySet;
        private bool _disposed;
            
        public EntityRepository(IContext context)
        {
            _context = context;
            _dbEntitySet = _context.Set<TEntity>();
        }

        public List<TEntity> GetAll()
        {
            return _dbEntitySet.ToList();
        }

        public List<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);

            return entities.ToList();
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbEntitySet.FirstOrDefaultAsync(predicate);
        }

        public TEntity GetSingleIncluding(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);

            return entities.FirstOrDefault(x => x.Id == id);
        }

        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntitySet.Where(predicate).ToList();
        }

        public void Insert(TEntity entity)
        {
            _context.SetAsAdded(entity);
        }

        public void Update(TEntity entity)
        {
            _context.SetAsModified(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.SetAsDeleted(entity);
        }

   //     public async Task Delete(long id)
   //     {
			//TEntity entity = await GetSingleAsync(id);
   //         _context.SetAsDeleted(entity);
   //     }
        //public void InsertAsync(TEntity entity) //REDO
        //{
        //    _context.SetAsAdded(entity);
        //    // return await _context.SaveChangesAsync();
        //}

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbEntitySet.ToListAsync();
        }

        // my
        public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate)
        {
			return await _dbEntitySet
            .Where(predicate)
            .CountAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(int count, int offset, Expression<Func<TEntity, bool>> predicate = null)
        {
			IQueryable<TEntity> entities = predicate != null ? _dbEntitySet.Where(predicate) : _dbEntitySet;
            entities = entities.Skip(offset).Take(count);
            return await entities.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;

            return await entities.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(int count, int offset,
            Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.Where(predicate) : entities;
            entities = entities.Skip(offset).Take(count);

            return await entities.ToListAsync();
        }


        // my

  //      public async Task<List<TEntity>> GetAllAsync(int count, int offset, Expression<Func<TEntity, DateTime>> keySelector,
			//	Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
		//{
		//	var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);

		//	entities = entities.Skip(offset).Take(count);

		//	return await entities.ToListAsync();
		//}

		//public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
  //      {
  //          var entities = IncludeProperties(includeProperties);
  //          return entities.ToListAsync();
  //      }

        public Task<TEntity> GetSingleAsync(long id)
        {
            return _dbEntitySet.FirstOrDefaultAsync(t => t.Id == id);
        }

        public Task<TEntity> GetSingleIncludingAsync(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            return entities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbEntitySet.Where(predicate).ToListAsync();
        }

        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_dbEntitySet, (current, includeProperty) => current.Include(includeProperty));
        }

		private IQueryable<TEntity> FilterQuery(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, DateTime>> keySelector, OrderBy orderBy, Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			entities = (predicate != null) ? entities.Where(predicate) : entities;
			entities = (orderBy == OrderBy.Ascending)
				? entities.OrderBy(keySelector)
				: entities.OrderByDescending(keySelector);

			return entities;
		}

		//private IQueryable<TEntity> FilterQuery(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>>[] includeProperties)
		//{
		//    var entities = IncludeProperties(includeProperties);
		//    entities = (predicate != null) ? entities.Where(predicate) : entities;
		//    return entities;
		//}

		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            // Do not dispose the context here; DI container manages context lifetime.
            if (!_disposed && disposing)
            {
                // no-op
            }
            _disposed = true;
        }
    }
}
