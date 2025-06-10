using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public record CreateReturnCommand : ICommand
{
    public required ClientId ClientId { get; init; }
    public required IEnumerable<ReturnProductCreateReturnCommand> Products { get; init; }

    public record ReturnProductCreateReturnCommand
    {
        public required ProductId ProductId { get; init; }
        public required int Quantity { get; init; }
    }
}