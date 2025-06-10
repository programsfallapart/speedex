namespace Speedex.Domain.Returns;

public record Return
{
    public required ReturnId ReturnId { get; init; }
    public required ClientId ClientId { get; init; }
    public required ReturnStatus ReturnStatus { get; init; }
    public required IEnumerable<ReturnProduct> Products { get; init; }
    public required DateTime CreationDate { get; init; }
    public required DateTime UpdateDate { get; init; }
}

public record ReturnProduct
{
    public required ProductId ProductId { get; init; }
    public required int Quantity { get; init; }
}

public enum ReturnStatus
{
    Created,
    Receipt,
    Qualified
}

public record ReturnId(string Value);

public record ProductId(string Value);

public record ClientId(string Value);
