using Pulse.WebAuthn.Domain.Shared;

namespace Pulse.WebAuthn.Domain.Infrastructure.Data
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Methods
        /// <summary>
        /// Gets an entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Gets an entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);
        #endregion

        #region Properties
        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }
        #endregion
    }
}
