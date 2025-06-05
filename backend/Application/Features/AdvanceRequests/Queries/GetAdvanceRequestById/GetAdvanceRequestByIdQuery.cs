using MediatR;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;

public sealed record GetAdvanceRequestByIdQuery(Guid Id) : IRequest<Result<GetAdvanceRequestByIdResponse>>;
