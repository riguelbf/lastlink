namespace Infrastructure.DataBase.Repositories.Base
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <returns>The repository for the entity type.</returns>
        IRepository<T>? Repository<T>() where T : class;

        /// <summary>
        /// Saves all changes made in this unit of work asynchronously.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        Task<int> SaveChangesAsync();
    }
}
