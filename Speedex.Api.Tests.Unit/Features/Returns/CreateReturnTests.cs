using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Speedex.Api.Features.Returns;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Api.Tests.Integration.Features.Returns;

public class ReturnsControllerTests
{
    [Fact]
    public void CreateReturn_Should_Return_Created_When_Command_Is_Successful()
    {
        // Arrange
        var returnId = new ReturnId("return123");
        var handler = Substitute.For<ICommandHandler<CreateReturnCommand, CreateReturnResult>>();
        handler.Handle(Arg.Any<CreateReturnCommand>(), Arg.Any<CancellationToken>())
            .Returns(new CreateReturnSuccess(new CreatedReturn{ReturnId = returnId}));

        var controller = new ReturnsController();
        var request = new CreateReturnBodyRequest
        {
            Products = new List<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct>
            {
                new() { ProductId = "product1", Quantity = 2 }
            }
        };

        // Act
        var result = controller.CreateReturn(request, handler);

        // Assert
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal($"/Returns?ReturnId={returnId.Value}", createdResult.Location);
    }

    [Fact]
    public void CreateReturn_Should_Return_BadRequest_When_Command_Has_ValidationError()
    {
        // Arrange
        var validationErrors = new List<ValidationError>
        {
            new() { Code = "INVALID_PRODUCT_ID", Message = "Product ID cannot be empty" }
        };
        
        var handler = Substitute.For<ICommandHandler<CreateReturnCommand, CreateReturnResult>>();
        handler.Handle(Arg.Any<CreateReturnCommand>(), Arg.Any<CancellationToken>())
            .Returns(new CreateReturnValidationError(validationErrors));

        var controller = new ReturnsController();
        var request = new CreateReturnBodyRequest
        {
            Products = new List<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct>
            {
                new() { ProductId = "", Quantity = 2 }
            }
        };

        // Act
        var result = controller.CreateReturn(request, handler);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        
        var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);
        Assert.Equal("Bad Request", problemDetails.Title);
        Assert.True(problemDetails.Extensions.ContainsKey("reasons"));
    }

    [Fact]
    public void CreateReturn_Should_Return_BadRequest_When_Command_Has_TechnicalError()
    {
        // Arrange
        var technicalError = new TechnicalError
        {
            Code = "ReturnCreationFailed",
            Message = "Failed to create return"
        };
        
        var handler = Substitute.For<ICommandHandler<CreateReturnCommand, CreateReturnResult>>();
        handler.Handle(Arg.Any<CreateReturnCommand>(), Arg.Any<CancellationToken>())
            .Returns(new CreateReturnTechnicalError(technicalError));

        var controller = new ReturnsController();
        var request = new CreateReturnBodyRequest
        {
            Products = new List<CreateReturnBodyRequest.CreateReturnBodyRequestReturnProduct>
            {
                new() { ProductId = "product1", Quantity = 2 }
            }
        };

        // Act
        var result = controller.CreateReturn(request, handler);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        
        var problemDetails = Assert.IsType<ProblemDetails>(badRequestResult.Value);
        Assert.Equal("Internal Server Error", problemDetails.Title);
        Assert.Equal(500, problemDetails.Status);
    }
}