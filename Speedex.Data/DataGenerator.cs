using Microsoft.Extensions.Options;
using Speedex.Domain.Returns;
using Speedex.Domain.Returns.Repositories;

namespace Speedex.Data;

public class DataGenerator(
    IDataGenerator<ReturnId, Return> returnGenerator,
    IReturnRepository returnRepository,
    IOptions<GenerateOptions> generateOptions)
    : IDataGenerator
{
    public void GenerateData()
    {
        var options = generateOptions.Value;

        returnGenerator.GenerateData(options.NbReturnElements);
        
        returnGenerator.Data.Values
            .ToList()
            .ForEach(x => returnRepository.UpsertReturn(x));
    }
}