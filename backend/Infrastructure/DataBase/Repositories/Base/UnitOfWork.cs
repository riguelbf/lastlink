using System.Collections;
using Infrastructure.DataBase.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DataBase.Repositories.Base
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Hashtable _repositories = new();
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // public IRepository<T> Repository<T>() where T : class
        // {
        //     var type = typeof(T).Name;
        //
        //     if (_repositories[type] is IRepository<T> repository)
        //         return repository;
        //
        //
        //     var repoType = typeof(Repository<>);
        //     var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), _context)!;
        //     _repositories[type] = repoInstance;
        //
        //     return (IRepository<T>)repoInstance;
        // }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            var type = typeof(TRepository).Name;

            if (_repositories[type] is TRepository repository)
                return repository;

            // Get the repository implementation from the service provider
            var repo = _serviceProvider.GetRequiredService<TRepository>();
            _repositories[type] = repo;
            
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _repositories.Clear();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}