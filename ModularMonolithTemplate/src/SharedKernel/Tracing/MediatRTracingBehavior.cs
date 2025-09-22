using System.Diagnostics;
using MediatR;

namespace ModularMonolithTemplate.SharedKernel.Tracing;

public sealed class MediatRTracingBehavior<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    where TReq : IRequest<TRes>
{
    private static readonly ActivitySource _source = new("App.MediatR");

    public async Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, CancellationToken ct)
    {
        var name = $"{request.GetType().Namespace}.{request.GetType().Name}";
        using var act = _source.StartActivity(name, ActivityKind.Internal);
        act?.SetTag("request.type", typeof(TReq).FullName);
        act?.SetTag("correlationId", Activity.Current?.TraceId.ToString());
        return await next();
    }
}
