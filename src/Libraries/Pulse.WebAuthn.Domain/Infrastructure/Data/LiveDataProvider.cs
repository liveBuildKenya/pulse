using Microsoft.EntityFrameworkCore;
using Pulse.WebAuthn.Domain.Shared;
using System.Reflection;

namespace Pulse.WebAuthn.Domain.Infrastructure.Data
{
    /// <summary>
    /// Represents the catalog data provider
    /// </summary>
    public class LiveDataProvider : DbContext, ILiveDataProvider
    {
        #region Constructor

        public LiveDataProvider(DbContextOptions<LiveDataProvider> options) : base(options)
        {

        }

        #endregion

        #region Utilities

        /// <summary>
        /// Further configuration of the model
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        #endregion

        #region Properties

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        #endregion
    }
}
