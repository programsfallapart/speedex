using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public record GetReturnsQueryResult : IQueryResult
{
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
    public required IEnumerable<Return> Items { get; init; }
}