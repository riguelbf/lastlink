namespace Infrastructure.DataBase.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        // /// <summary>
        // /// Gets the repository for the specified entity type.
        // /// </summary>
        // /// <typeparam name="T">The entity type.</typeparam>
        // /// <returns>The repository for the entity type.</returns>
        // IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Gets the specific repository for the specified repository interface.
        /// </summary>
        /// <typeparam name="TRepository">The repository interface type.</typeparam>
        /// <returns>The repository implementation.</returns>
        TRepository GetRepository<TRepository>() where TRepository : class;

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
