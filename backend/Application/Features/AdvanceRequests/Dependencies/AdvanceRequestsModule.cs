using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;
using Application.Features.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestById;
using Application.Features.AdvanceRequests.Queries.GetAdvanceRequests;
using Application.Features.AdvanceRequests.Queries.GetAdvanceRequestsByCreator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Primitives;

namespace Application.Features.AdvanceRequests.Dependencies;

public static class AdvanceRequestsModule
{
    public static IServiceCollection AddAdvanceRequestsModule(this IServiceCollection services)
    {
        // Register command and query handlers
        services.AddScoped<IRequestHandler<CreateAdvanceRequestCommand, Result<CreateAdvanceRequestResponse>>, CreateAdvanceRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GetAdvanceRequestByIdQuery, Result<GetAdvanceRequestByIdResponse>>, GetAdvanceRequestByIdQueryHandler>();
        services.AddScoped<IRequestHandler<GetAdvanceRequestsQuery, Result<GetAdvanceRequestsResponse>>, GetAdvanceRequestsQueryHandler>();
        services.AddScoped<IRequestHandler<GetAdvanceRequestsByCreatorQuery, Result<IReadOnlyList<GetAdvanceRequestsByCreatorResponse>>>, GetAdvanceRequestsByCreatorQueryHandler>();
        services.AddScoped<IRequestHandler<ApproveAdvanceRequestCommand, Result>, UpdateAdvanceRequestStatusCommandHandler>();
        services.AddScoped<IRequestHandler<RejectAdvanceRequestCommand, Result>, UpdateAdvanceRequestStatusCommandHandler>();
        
        return services;
    }
}
