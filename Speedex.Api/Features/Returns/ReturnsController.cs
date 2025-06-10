using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Commons;
using Speedex.Api.Features.Returns.Mappers;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns.UseCases.CreateReturn;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Features.Returns;

[ApiController]
[Route("[controller]")]
public class ReturnsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateReturn(
        [FromBody] CreateReturnBodyRequest bodyRequest,
        [FromServices] ICommandHandler<CreateReturnCommand, CreateReturnResult> handler)
    {
        var commandResult = handler.Handle(bodyRequest.ToCommand());
        
        return commandResult.Match<IActionResult>(
            onSuccess: success => Created(
                $"/Returns?ReturnId={success.Value.ReturnId?.Value}", null),
            onFailure: error => error switch
            {
                CreateReturnValidationError validationError => HandleValidationError(validationError),
                CreateReturnTechnicalError technicalError => HandleTechnicalError(technicalError),
            }
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetReturns(
        [FromQuery] GetReturnsQueryParams queryParams,
        [FromServices] IQueryHandler<GetReturnsQuery, GetReturnsQueryResult> handler)
    {
        var result = await handler.Handle(queryParams.ToQuery());

        return Ok(result.ToResponse());
    }
    
    private BadRequestObjectResult HandleValidationError(CreateReturnValidationError validationError)
    {
        var problemDetails = validationError.Errors.ToBadRequest();
        return BadRequest(problemDetails);
    }
    
    private BadRequestObjectResult HandleTechnicalError(CreateReturnTechnicalError validationError)
    {
        var problemDetails = validationError.Error.ToInternalServerError();
        return BadRequest(problemDetails);
    }
}