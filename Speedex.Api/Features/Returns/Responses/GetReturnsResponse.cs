namespace Speedex.Api.Features.Returns.Responses;

public class GetReturnsResponse
{
    public required int PageSize { get; init; }
    public required int PageIndex { get; init; }
    public required IEnumerable<GetReturnItemResponse> Items { get; init; }

    public class GetReturnItemResponse
    {
        public required string ReturnId { get; init; }
        public required string Status { get; init; }
        public required string CreationDate { get; init; }
        public required string UpdateDate { get; init; }
        public required string ClientId { get; init; }
        public required IEnumerable<ReturnProductGetReturnItemResponse> Products { get; init; }

        public record ReturnProductGetReturnItemResponse
        {
            public required string ProductId { get; init; }
            public required int Quantity { get; init; }
        }
    }
}