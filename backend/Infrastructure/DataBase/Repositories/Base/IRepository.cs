using System.Linq.Expressions;

namespace Infrastructure.DataBase.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its primary key asynchronously.
        /// </summary>
        /// <param name="id">The entity primary key.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The entity if found.</returns>
        Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds entities matching the given predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The filter expression.</param>
        /// <returns>A list of matching entities.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Removes an entity.  
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);
    }
}
