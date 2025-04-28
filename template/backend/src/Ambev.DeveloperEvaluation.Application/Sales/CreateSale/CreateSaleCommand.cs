using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale
/// </summary>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the ID of the customer making the purchase
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the branch where the sale is made
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the items in the sale
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = new();
}

/// <summary>
/// Command for creating a sale item within a sale
/// </summary>
public class CreateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the ID of the item being sold
    /// </summary>
    public Guid ItemId { get; set; }

    /// <summary>
    /// Gets or sets the quantity of the item
    /// </summary>
    public int Quantity { get; set; }
} 