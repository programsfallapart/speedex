using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandValidator : IValidator<CreateReturnCommand>
{
    public ValidationResult<CreateReturnCommand> Validate(CreateReturnCommand command)
    {
        var productsArray = command.Products.ToArray();

        var validationErrors = ValidateProductList(productsArray)
            .Concat(ValidateProductIds(productsArray))
            .Concat(ValidateQuantities(productsArray))
            .Concat(ValidateDuplicateProducts(productsArray))
            .ToArray();

        return validationErrors.Length != 0
            ? Invalid<CreateReturnCommand>.Create(validationErrors)
            : Valid<CreateReturnCommand>.Create(command);
    }

    private static IEnumerable<ValidationError> ValidateProductList(
        CreateReturnCommand.ReturnProductCreateReturnCommand[] products)
    {
        if (products.Length == 0)
        {
            yield return new ValidationError
            {
                Code = "INVALID_PRODUCT_LIST",
                Message = "The Products list cannot be null or empty."
            };
        }
    }

    private static IEnumerable<ValidationError> ValidateProductIds(
        CreateReturnCommand.ReturnProductCreateReturnCommand[] products)
    {
        return products
            .Select((product, index) => new { product, index })
            .Where(x => string.IsNullOrWhiteSpace(x.product.ProductId.Value))
            .Select(invalid => new ValidationError
            {
                Code = "INVALID_PRODUCT_ID",
                Message = $"Product ID at index {invalid.index} cannot be null or empty."
            });
    }

    private static IEnumerable<ValidationError> ValidateQuantities(
        CreateReturnCommand.ReturnProductCreateReturnCommand[] products)
    {
        return products
            .Select((product, index) => new { product, index })
            .Where(x => x.product.Quantity <= 0)
            .Select(invalid => new ValidationError
            {
                Code = "INVALID_QUANTITY",
                Message = $"Quantity at index {invalid.index} must be greater than zero. Current value: {invalid.product.Quantity}."
            });
    }

    private static IEnumerable<ValidationError> ValidateDuplicateProducts(
        CreateReturnCommand.ReturnProductCreateReturnCommand[] products)
    {
        var duplicateProductIds = products
            .Where(product => !string.IsNullOrWhiteSpace(product.ProductId.Value))
            .GroupBy(product => product.ProductId.Value, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToArray();

        if (duplicateProductIds.Length != 0)
        {
            yield return new ValidationError
            {
                Code = "DUPLICATE_PRODUCT_IDS",
                Message = $"Duplicate Product IDs found: {string.Join(", ", duplicateProductIds)}"
            };
        }
    }
}