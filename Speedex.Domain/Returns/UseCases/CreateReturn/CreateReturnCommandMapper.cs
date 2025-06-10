using Speedex.Domain.Commons;

namespace Speedex.Domain.Returns.UseCases.CreateReturn;

public static class CreateReturnCommandMapper
{
    public static Return ToReturn(this Valid<CreateReturnCommand> command)
    {
        var now = DateTime.Now;

        var createdReturn = new Return
        {
            ReturnId = new ReturnId(Guid.NewGuid().ToString()),
            ReturnStatus = ReturnStatus.Created,
            CreationDate = now,
            UpdateDate = now,
            ClientId = command.Value.ClientId,
            Products = command.Value.Products.Select(
                x => new ReturnProduct
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity
                })
        };
        return createdReturn;
    }
}