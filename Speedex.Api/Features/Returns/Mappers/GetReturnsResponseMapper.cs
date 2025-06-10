using Speedex.Api.Features.Returns.Responses;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Features.Returns.Mappers;

public static class GetReturnsResponseMapper
{
    public static GetReturnsResponse ToResponse(this GetReturnsQueryResult result)
    {
        return new GetReturnsResponse
        {
            PageIndex = result.PageIndex,
            PageSize = result.PageSize,
            Items = result.Items.Select(x => new GetReturnsResponse.GetReturnItemResponse
            {
                ReturnId = x.ReturnId.Value,
                Status = x.ReturnStatus.ToString(),
                CreationDate = x.CreationDate.ToString("u"),
                UpdateDate = x.UpdateDate.ToString("u"),
                ClientId = x.ClientId.Value,
                Products = x.Products.Select(p => new GetReturnsResponse.GetReturnItemResponse.ReturnProductGetReturnItemResponse
                {
                    ProductId = p.ProductId.Value,
                    Quantity = p.Quantity
                })
            })
        };
    }
}