using Domain.AdvanceRequests.Repositories;
using Infrastructure.DataBase.DataContext;
using Microsoft.EntityFrameworkCore;
using DomainAdvanceRequest = Domain.AdvanceRequests.AdvanceRequest;

namespace Infrastructure.DataBase.Repositories;

internal class AdvanceRequestRepository : IAdvanceRequestRepository
{
    private readonly ApplicationDbContext _dbContext;
    private IAdvanceRequestRepository? _advanceRequestRepositoryImplementation;

    public AdvanceRequestRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IReadOnlyList<DomainAdvanceRequest>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(DomainAdvanceRequest advanceRequest, CancellationToken cancellationToken = default)
    {
        await _dbContext.AdvanceRequests.AddAsync(advanceRequest, cancellationToken);
    }

    public void Update(DomainAdvanceRequest entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(DomainAdvanceRequest entity)
    {
        throw new NotImplementedException();
    }

    public async Task<DomainAdvanceRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AdvanceRequests
            .FirstOrDefaultAsync(ar => ar.Id == id, cancellationToken);
    }

    public async Task<List<DomainAdvanceRequest>> GetByCreatorIdAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AdvanceRequests
            .Where(ar => ar.CreatorId == creatorId)
            .OrderByDescending(ar => ar.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasPendingRequestAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AdvanceRequests
            .AnyAsync(ar => 
                ar.CreatorId == creatorId && 
                ar.Status == Domain.AdvanceRequests.Enums.AdvanceRequestStatus.Pending, 
                cancellationToken);
    }
}
