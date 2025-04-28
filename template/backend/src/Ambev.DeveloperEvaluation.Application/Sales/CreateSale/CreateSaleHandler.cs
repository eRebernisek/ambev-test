using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for creating a new sale
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IItemRepository _itemRepository;

    /// <summary>
    /// Initializes a new instance of the CreateSaleHandler class
    /// </summary>
    public CreateSaleHandler(ISaleRepository saleRepository, IItemRepository itemRepository)
    {
        _saleRepository = saleRepository;
        _itemRepository = itemRepository;
    }

    /// <summary>
    /// Handles the creation of a new sale
    /// </summary>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Create new sale entity
        var sale = new Sale
        {
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            SaleDate = DateTime.UtcNow,
            Items = new List<SaleItem>()
        };

        // Process each item in the sale
        foreach (var itemCommand in request.Items)
        {
            // Get the item to validate it exists and get its current price
            var item = await _itemRepository.GetByIdAsync(itemCommand.ItemId);
            if (item == null)
                throw new InvalidOperationException($"Item with ID {itemCommand.ItemId} not found");

            // Validate stock availability
            if (!item.UpdateStock(itemCommand.Quantity))
                throw new InvalidOperationException($"Insufficient stock for item {item.Name}");

            // Create sale item
            var saleItem = new SaleItem
            {
                ItemId = item.Id,
                Quantity = itemCommand.Quantity,
                UnitPrice = item.Price
            };

            // Calculate discount based on quantity
            saleItem.CalculateDiscount();
            
            // Add item to sale
            sale.Items.Add(saleItem);

            // Update item stock in database
            await _itemRepository.UpdateAsync(item);
        }

        // Calculate sale totals
        sale.CalculateTotals();

        // Validate the sale
        var validation = sale.Validate();
        if (!validation.IsValid)
            throw new InvalidOperationException($"Invalid sale: {string.Join(", ", validation.Errors)}");

        // Save the sale
        var createdSale = await _saleRepository.CreateAsync(sale);

        // Create result
        var result = new CreateSaleResult
        {
            Id = createdSale.Id,
            CustomerId = createdSale.CustomerId,
            BranchId = createdSale.BranchId,
            TotalAmount = createdSale.TotalPrice + createdSale.TotalDiscount,
            TotalDiscount = createdSale.TotalDiscount,
            FinalAmount = createdSale.TotalPrice,
            CreatedAt = createdSale.CreatedAt,
            Items = createdSale.Items.Select(i => new CreateSaleItemResult
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercentage = i.Discount / (i.UnitPrice * i.Quantity) * 100,
                DiscountAmount = i.Discount,
                FinalAmount = i.TotalPrice
            }).ToList()
        };

        return result;
    }
} 