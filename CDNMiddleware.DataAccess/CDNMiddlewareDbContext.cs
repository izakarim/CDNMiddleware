using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using CDNMiddleware.DataAccess.Models;

namespace CDNMiddleware.DataAccess
{
	public class CDNMiddlewareDbContext : DbContext
	{
		public CDNMiddlewareDbContext(DbContextOptions<CDNMiddlewareDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            base.OnModelCreating(modelBuilder);

            #region Many-to-many relationship
            #endregion

            #region One-to-many relationship
            #endregion

            #region Custom Relationship
            #endregion
        }

        // reference: https://stackoverflow.com/q/38998535/3335442
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.Now;

            foreach (var entry in entries)
            {
                if (entry.Entity is Base trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedAt = utcNow;
                            break;
                        case EntityState.Added:
                            trackable.CreatedAt = utcNow;
                            trackable.UpdatedAt = utcNow;
                            break;
                        case EntityState.Deleted:
                            if (trackable.DeletedAt is null)
                            {
                                //https://www.ryansouthgate.com/2019/01/07/entity-framework-core-soft-delete/
                                // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                                entry.State = EntityState.Unchanged;
                                trackable.DeletedAt = utcNow;
                            }
                            break;
                    }
                }
            }
        }

        #region DbSet
        public DbSet<User> Users { get; set; }
        #endregion
    }
}

