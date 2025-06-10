using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public class CreateReturnCommandHandler : ICommandHandler<CreateReturnCommand, CreateReturnResult>
{
    private readonly IReturnRepository _returnRepository;
    private readonly IValidator<CreateReturnCommand> _commandValidator;

    public CreateReturnCommandHandler(IReturnRepository returnRepository, IValidator<CreateReturnCommand> commandValidator)
    {
        _returnRepository = returnRepository;
        _commandValidator = commandValidator;
    }

    public CreateReturnResult Handle(CreateReturnCommand command, CancellationToken cancellationToken = default)
    {
        return _commandValidator.Validate(command).Match<CreateReturnResult>(
            valid => {
                var createdReturn = valid.ToReturn();
                var upsertResult = _returnRepository.UpsertReturn(createdReturn);
            
                return upsertResult.Status == UpsertReturnResult.UpsertStatus.Success
                    ? new CreateReturnSuccess(new CreatedReturn{ReturnId = createdReturn.ReturnId})
                    : new CreateReturnTechnicalError(new TechnicalError
                    {
                        Code = "ReturnCreationFailed",
                        Message = $"Failed to create return: {createdReturn.ReturnId}"
                    });
            },
            invalid => new CreateReturnValidationError(invalid.Errors.ToList())
        );
    }
}