using System.Collections.Concurrent;
using Speedex.Domain.Returns;

namespace Speedex.Data.Generators;

public class ReturnsGenerator : IDataGenerator<ReturnId, Return>
{
    public Dictionary<ReturnId, Return> Data { get; private set; }
    private readonly Random _random = new();

    public void GenerateData(int nbElements)
    {
        var concurrentData = new ConcurrentDictionary<ReturnId, Return>();

        Enumerable
            .Range(0, nbElements)
            .AsParallel()
            .ForAll(
                x =>
                {
                    var product = GenerateReturn(x);
                    concurrentData.TryAdd(product.ReturnId, product);
                });

        Data = concurrentData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private Return GenerateReturn(int index)
    {
        return new Return
        {
            ReturnId = new ReturnId($"RE_{index}_{GenerateHexadecimal(10)}"),
            ReturnStatus = (ReturnStatus)_random.Next(0, 3),
            CreationDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            ClientId = new ClientId($"CL_{index}_{GenerateHexadecimal(10)}"),
            Products = [new ReturnProduct()
            {
                ProductId = new ProductId($"PR_{index}_{GenerateHexadecimal(10)}"),
                Quantity = 3,
            }]
        };
    }
}