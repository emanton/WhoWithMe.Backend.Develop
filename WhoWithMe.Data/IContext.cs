using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WhoWithMe.Core.Data;

namespace WhoWithMe.Data
{
    public interface IContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
