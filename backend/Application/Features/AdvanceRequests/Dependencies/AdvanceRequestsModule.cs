using Application.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Application.Features.AdvanceRequests.Commands.CreateAdvanceRequest;
using Application.Features.AdvanceRequests.Commands.UpdateAdvanceRequestStatus;
using Application.Features.AdvanceRequests.Queries.GetAdvanceRequests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Primitives;

namespace Application.AdvanceRequests.Dependencies;

public static class AdvanceRequestsModule
{
    public static IServiceCollection AddAdvanceRequestsModule(this IServiceCollection services)
    {
        // Register command and query handlers
        services.AddScoped<IRequestHandler<CreateAdvanceRequestCommand, Result<CreateAdvanceRequestResponse>>, CreateAdvanceRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GetAdvanceRequestsQuery, Result<GetAdvanceRequestsResponse>>, GetAdvanceRequestsQueryHandler>();
        services.AddScoped<IRequestHandler<ApproveAdvanceRequestCommand, Result>, UpdateAdvanceRequestStatusCommandHandler>();
        services.AddScoped<IRequestHandler<RejectAdvanceRequestCommand, Result>, UpdateAdvanceRequestStatusCommandHandler>();
        
        return services;
    }
}
