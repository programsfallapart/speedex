using Speedex.Domain.Commons;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Domain.Returns.UseCases.GetReturns;

public class GetReturnsQueryHandler(IReturnRepository returnRepository) : IQueryHandler<GetReturnsQuery, GetReturnsQueryResult>
{
    public Task<GetReturnsQueryResult> Handle(GetReturnsQuery query, CancellationToken cancellationToken = default)
    {
        var getReturnsDto = query.ToGetReturnsDto();
        
        var result = returnRepository.GetReturns(getReturnsDto);

        return Task.FromResult(new GetReturnsQueryResult
        {
            PageIndex = getReturnsDto.PageIndex,
            PageSize = getReturnsDto.PageSize,
            Items = result
        });
    }
}