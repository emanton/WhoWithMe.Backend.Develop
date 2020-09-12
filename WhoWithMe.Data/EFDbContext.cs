using WhoWithMe.Core.Entities;
using WhoWithMe.Core.Entities.dictionaries;
using WhoWithMe.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace WhoWithMe.Data
{
    public class EFDbContext : DbContext, IContext
    {
        public EFDbContext()
                : base()
        {
        }

        public EFDbContext(DbContextOptions<EFDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeetingSubscriber>()
            .HasOne(b => b.Meeting)
            .WithMany(a => a.MeetingSubscribers)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MeetingSubscriber>()
            .HasOne(b => b.User)
            .WithMany(a => a.MeetingSubscribers)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<City>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            modelBuilder.Entity<MeetingType>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
			modelBuilder.Entity<MeetingSubscriber>(entity =>
			{
				entity.HasIndex(e => new { e.MeetingId, e.UserId }).IsUnique();
			});

            

            base.OnModelCreating(modelBuilder);
        }
        
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<MeetingImage> MeetingImage { get; set; }
        public virtual DbSet<UserImage> UserImage { get; set; }
        public virtual DbSet<MeetingType> MeetingType { get; set; }        
        public virtual DbSet<CommentMeeting> CommentMeeting { get; set; }
        public virtual DbSet<CommentUser> CommentUser { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Meeting> Meeting { get; set; }
        public virtual DbSet<MeetingSubscriber> MeetingSubscriber { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<UnreadMessage> UnreadMessage { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserChat> UserChat { get; set; }
        public virtual DbSet<UserSubscriber> UserSubscriber { get; set; }
        //public virtual DbSet<UserPhoneToConfirm> UserPhoneToConfirm { get; set; }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : class, IBaseEntity
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private EntityEntry<TEntity> GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
			EntityEntry<TEntity> dbEntityEntry = Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }

            return dbEntityEntry;
        }

		
	}
}
