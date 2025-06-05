using SharedKernel.Primitives;

namespace Application.Base.Commands;

public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<Result<ICommand>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}