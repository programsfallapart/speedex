namespace Speedex.Domain.Commons;

public interface ICommandHandler<in TCommand, out TCommandResult> where TCommand : ICommand
{
    TCommandResult Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommand
{
}
