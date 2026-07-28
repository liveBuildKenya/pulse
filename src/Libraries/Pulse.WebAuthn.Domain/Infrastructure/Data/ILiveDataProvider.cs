using Microsoft.EntityFrameworkCore;
using Pulse.WebAuthn.Domain.Shared;

namespace Pulse.WebAuthn.Domain.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context
    /// </summary>
    public interface ILiveDataProvider
    {
        /// <summary>
        /// Creates a DbSet that can be used to query and save instances of an entity
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <returns>A set for the given type</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Saves all changes made in this context to the database
        /// </summary>
        /// <returns>The number of state entries written to the database</returns>
        int SaveChanges();
    }
}
