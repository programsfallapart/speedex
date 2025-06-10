using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;
using Speedex.Domain.Returns.Repositories.Dtos;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Domain.Tests.Unit.Returns.CreateReturn;

public class CreateReturnTests
{
    [Fact]
    public void Handle_Should_Return_Success_Result_When_Return_Is_Created_Successfully()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();
        
        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products =
            [
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId("product1"),
                    Quantity = 2
                }
            ]
        };
        
        Return? capturedReturn = null; 
        returnRepository .UpsertReturn(Arg.Do<Return>(r => capturedReturn = r)) .Returns(new UpsertReturnResult { Status = UpsertReturnResult.UpsertStatus.Success });

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnSuccess>(result);
        var successResult = (CreateReturnSuccess)result;
        Assert.NotNull(successResult.Value.ReturnId);
        Assert.NotNull(capturedReturn);
        Assert.NotNull(capturedReturn.ReturnId);
        Assert.NotEmpty(capturedReturn.ReturnId.Value);
        Assert.Equal(ReturnStatus.Created, capturedReturn.ReturnStatus);
    
        var products = capturedReturn.Products.ToList();
        Assert.Single(products);
        Assert.Equal("product1", products[0].ProductId.Value);
        Assert.Equal(2, products[0].Quantity);
    
        Assert.True(capturedReturn.CreationDate.Date == DateTime.Now.Date);
        Assert.True(capturedReturn.UpdateDate.Date == DateTime.Now.Date);
    }

    [Fact]
    public void Handle_Should_Return_ValidationError_When_Products_List_Is_Empty()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();

        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products = []
        };

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnValidationError>(result);
        var errorResult = (CreateReturnValidationError)result;
        Assert.Single(errorResult.Errors);
        Assert.Equal("INVALID_PRODUCT_LIST", errorResult.Errors[0].Code);
        Assert.Contains("cannot be null or empty", errorResult.Errors[0].Message);

        returnRepository.DidNotReceive().UpsertReturn(Arg.Any<Return>());
    }

    [Fact]
    public void Handle_Should_Return_ValidationError_When_Product_Has_Invalid_Id()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();

        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products = [
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId(""),
                    Quantity = 1
                }
            ]
        };

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnValidationError>(result);
        var errorResult = (CreateReturnValidationError)result;
        Assert.Contains(errorResult.Errors, e => e.Code == "INVALID_PRODUCT_ID");
        
        returnRepository.DidNotReceive().UpsertReturn(Arg.Any<Return>());
    }

    [Fact]
    public void Handle_Should_Return_ValidationError_When_Product_Has_Invalid_Quantity()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();

        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products = [
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId("product1"),
                    Quantity = 0
                }
            ]
        };

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnValidationError>(result);
        var errorResult = (CreateReturnValidationError)result;
        Assert.Contains(errorResult.Errors, e => e.Code == "INVALID_QUANTITY");
        Assert.Contains(errorResult.Errors, e => e.Message.Contains("must be greater than zero"));
        
        returnRepository.DidNotReceive().UpsertReturn(Arg.Any<Return>());
    }

    [Fact]
    public void Handle_Should_Return_ValidationError_When_Products_Have_Duplicate_Ids()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();

        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products = [
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId("product1"),
                    Quantity = 1
                },
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId("product1"),
                    Quantity = 2
                }
            ]
        };

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnValidationError>(result);
        var errorResult = (CreateReturnValidationError)result;
        Assert.Contains(errorResult.Errors, e => e.Code == "DUPLICATE_PRODUCT_IDS");
        Assert.Contains(errorResult.Errors, e => e.Message.Contains("product1"));
        
        returnRepository.DidNotReceive().UpsertReturn(Arg.Any<Return>());
    }

    [Fact]
    public void Handle_Should_Return_TechnicalError_When_Repository_Operation_Fails()
    {
        // Arrange
        var returnRepository = Substitute.For<IReturnRepository>();
        var commandValidator = new CreateReturnCommandValidator();

        var command = new CreateReturnCommand
        {
            ClientId = new ClientId("clientId"),
            Products = [
                new CreateReturnCommand.ReturnProductCreateReturnCommand
                {
                    ProductId = new ProductId("product1"),
                    Quantity = 2
                }
            ]
        };

        returnRepository
            .UpsertReturn(Arg.Any<Return>())
            .Returns(new UpsertReturnResult { Status = UpsertReturnResult.UpsertStatus.Failed });

        var handler = new CreateReturnCommandHandler(returnRepository, commandValidator);

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.IsType<CreateReturnTechnicalError>(result);
        var errorResult = (CreateReturnTechnicalError)result;
        Assert.Equal("ReturnCreationFailed", errorResult.Error.Code);
    }
}