namespace Speedex.Domain.Commons;

public record TechnicalError
{
    public required string Code { get; init; }
    public required string Message { get; init; }
}