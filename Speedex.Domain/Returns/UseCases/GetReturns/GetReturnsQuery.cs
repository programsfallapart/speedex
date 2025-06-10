using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public record GetReturnsQuery : IQuery
{
    public required ReturnId? ReturnId { get; init; }
    public required int? PageIndex { get; init; }
    public required int? PageSize { get; init; }
}