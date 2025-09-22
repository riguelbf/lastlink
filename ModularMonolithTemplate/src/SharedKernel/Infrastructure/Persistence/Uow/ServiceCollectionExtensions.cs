using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.Uow;

namespace ModularMonolithTemplate.SharedKernel.Infrastructure.Persistence.Uow;

public static class ServiceCollectionExtensions
{
    // Registers UnitOfWork for a write DbContext and exposes FluentUnitOfWork for transactional application flows.
    public static IServiceCollection AddUnitOfWork<TWriteDbContext, TReadDbContext>(this IServiceCollection services)
        where TWriteDbContext : DbContext
        where TReadDbContext : DbContext
    {
        services.AddScoped<IUnitOfWork, EfCoreUnitOfWork<TWriteDbContext>>();
        services.AddScoped(provider =>
            new FluentUnitOfWork<TWriteDbContext>(
                provider.GetRequiredService<IUnitOfWork>(),
                provider.GetRequiredService<TWriteDbContext>()));
        // ReadDbContext is expected to be registered by the module; this method enforces the generic pair at compile time.
        return services;
    }
}
