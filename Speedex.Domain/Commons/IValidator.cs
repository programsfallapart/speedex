namespace Speedex.Domain.Commons;

public interface IValidator<T>
{
    ValidationResult<T> Validate(T item);
}