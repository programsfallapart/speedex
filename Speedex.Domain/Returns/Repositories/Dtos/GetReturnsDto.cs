namespace Speedex.Domain.Returns.Repositories.Dtos;

public record GetReturnsDto
{
    public required ReturnId? ReturnId { get; init; }
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
}