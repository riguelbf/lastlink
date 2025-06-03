using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        /// <summary>
        /// Gets an entity by its primary key asynchronously.
        /// </summary>
        /// <param name="id">The entity primary key.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The entity if found; otherwise, throws InvalidOperationException.</returns>
        public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken) => await DbSet.FindAsync(id, cancellationToken) ?? throw new InvalidOperationException();

        /// <summary>
        /// Gets all entities asynchronously.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.ToListAsync();

        /// <summary>
        /// Finds entities matching the given predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The filter expression.</param>
        /// <returns>A list of matching entities.</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<System.Func<T, bool>> predicate)
            => await Task.FromResult(DbSet.Where(predicate));

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(T entity) => DbSet.Update(entity);

        /// <summary>
        /// Removes an entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(T entity) => DbSet.Remove(entity);
    }
}
