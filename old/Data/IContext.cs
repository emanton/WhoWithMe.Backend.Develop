using Core.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Data
{
    public interface IContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbTransaction BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> CommitAsync();
    }
}
