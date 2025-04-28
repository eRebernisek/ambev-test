using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides test data for CreateSaleHandler tests
/// </summary>
public static class CreateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid sale commands
    /// </summary>
    private static readonly Faker<CreateSaleCommand> CreateSaleCommandFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.Items, f => GenerateItems(f.Random.Number(1, 5)));

    /// <summary>
    /// Configures the Faker to generate valid sale item commands
    /// </summary>
    private static readonly Faker<CreateSaleItemCommand> CreateSaleItemCommandFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ItemId, f => f.Random.Guid())
        .RuleFor(i => i.Quantity, f => f.Random.Number(1, 15));

    /// <summary>
    /// Generates a valid sale command with random data
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return CreateSaleCommandFaker.Generate();
    }

    /// <summary>
    /// Generates a list of sale item commands
    /// </summary>
    private static List<CreateSaleItemCommand> GenerateItems(int count)
    {
        return CreateSaleItemCommandFaker.Generate(count);
    }
} 