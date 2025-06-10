using Microsoft.AspNetCore.Mvc;
using Speedex.Api.Features.Returns.Requests;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.UseCases.CreateReturn;

namespace Speedex.Api.Features.Returns.Mappers;

public static class CreateReturnBodyRequestMapper
{
    public static CreateReturnCommand ToCommand(this CreateReturnBodyRequest bodyRequest)
    {
        return new CreateReturnCommand
        {
            ClientId = new ClientId(bodyRequest.ClientId!),
            Products = bodyRequest.Products!.Select(x => new CreateReturnCommand.ReturnProductCreateReturnCommand
            {
                ProductId = new ProductId(x.ProductId!),
                Quantity = x.Quantity!.Value
            })
        };
    }
}