namespace Speedex.Domain.Commons;

public abstract record ValidationResult<T> : Result<Valid<T>, Invalid<T>>;

public sealed record Valid<T> : ValidationResult<T>
{
    public required T Value { get; init; }
    public static Valid<T> Create(T value) => new() { Value = value };
}

public sealed record Invalid<T> : ValidationResult<T>
{
    public required IEnumerable<ValidationError> Errors { get; init; }
    
    public static Invalid<T> Create(IEnumerable<ValidationError> errors)
        => new() { Errors = errors };
}

public sealed record ValidationError
{
    public required string Code { get; init; }
    public required string Message { get; init; }
}