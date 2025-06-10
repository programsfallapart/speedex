using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public abstract record CreateReturnResult : Result<CreateReturnSuccess, CreateReturnError> { }

public record CreateReturnSuccess(CreatedReturn Return) : CreateReturnResult
{
    public CreatedReturn Value => Return;
}

public abstract record CreateReturnError : CreateReturnResult;

public record CreateReturnValidationError(IReadOnlyList<ValidationError> Errors) : CreateReturnError;

public record CreateReturnTechnicalError(TechnicalError Error) : CreateReturnError;


public record CreatedReturn
{ 
    public required ReturnId ReturnId { get; init; }
}