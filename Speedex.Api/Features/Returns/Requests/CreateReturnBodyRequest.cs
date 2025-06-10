using System.ComponentModel.DataAnnotations;

namespace Speedex.Api.Features.Returns.Requests;

public record CreateReturnBodyRequest
{ 
    [Required]
    public string? ClientId { get; init; }
    [Required]
    public IEnumerable<CreateReturnBodyRequestReturnProduct>? Products { get; init; }

    public record CreateReturnBodyRequestReturnProduct
    {
        [Required]
        public string? ProductId { get; init; }
        [Required]
        public int? Quantity { get; init; }
    }
}