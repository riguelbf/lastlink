using System.Collections;
using Infrastructure.DataBase.DataContext;

namespace Infrastructure.DataBase.Repositories.Base
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly Hashtable _repositories = new();
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (_repositories.ContainsKey(type))
                return (IRepository<T>)_repositories[type]!;

            var repoType = typeof(Repository<>);
            var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(T)), _context)!;
            _repositories.Add(type, repoInstance);

            return (IRepository<T>)repoInstance;
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