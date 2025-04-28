using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing sale creation commands.
/// </summary>
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSaleCommandHandler(
        ISaleRepository saleRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository,
        IItemRepository itemRepository,
        IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        // Validate existence of customer and branch
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new InvalidOperationException($"Customer with ID {request.CustomerId} not found");

        var branch = await _branchRepository.GetByIdAsync(request.BranchId)
            ?? throw new InvalidOperationException($"Branch with ID {request.BranchId} not found");

        // Create sale entity
        var sale = new Sale
        {
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            CreatedAt = DateTime.UtcNow,
            SaleDate = DateTime.UtcNow
        };

        // Process items
        foreach (var itemRequest in request.Items)
        {
            var item = await _itemRepository.GetByIdAsync(itemRequest.ItemId)
                ?? throw new InvalidOperationException($"Item with ID {itemRequest.ItemId} not found");

            if (item.QuantityInStock < itemRequest.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for item {item.Name}. Available: {item.QuantityInStock}, Requested: {itemRequest.Quantity}");
            }

            // Update stock
            item.QuantityInStock -= itemRequest.Quantity;

            // Add sale item
            sale.Items.Add(new SaleItem
            {
                ItemId = item.Id,
                Quantity = itemRequest.Quantity,
                UnitPrice = item.Price,
                TotalPrice = item.Price * itemRequest.Quantity
            });
        }

        // Calculate totals
        sale.CalculateTotals();

        // Validate sale
        var validationResult = sale.Validate();
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(
                $"Sale validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.Detail))}");
        }

        // Save changes
        sale = await _saleRepository.CreateAsync(sale);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Return result
        return new CreateSaleResult
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            BranchId = sale.BranchId,
            CreatedAt = sale.CreatedAt,
            TotalAmount = sale.TotalPrice + sale.TotalDiscount,
            TotalDiscount = sale.TotalDiscount,
            FinalAmount = sale.TotalPrice,
            Items = sale.Items.Select(i => new CreateSaleItemResult
            {
                ItemId = i.ItemId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                DiscountPercentage = i.Discount / (i.UnitPrice * i.Quantity) * 100,
                DiscountAmount = i.Discount,
                FinalAmount = i.TotalPrice
            }).ToList()
        };
    }
} 