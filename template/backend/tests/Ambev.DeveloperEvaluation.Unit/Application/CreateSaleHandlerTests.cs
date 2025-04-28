using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the CreateSaleHandler class
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IItemRepository _itemRepository;
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _itemRepository = Substitute.For<IItemRepository>();
        _handler = new CreateSaleHandler(_saleRepository, _itemRepository);
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var items = command.Items.Select(i => new Item
        {
            Id = i.ItemId,
            Name = "Test Item",
            Price = 10.0m,
            QuantityInStock = 20
        }).ToList();

        foreach (var item in items)
        {
            _itemRepository.GetByIdAsync(item.Id)
                .Returns(item);
        }

        _saleRepository.CreateAsync(Arg.Any<Sale>())
            .Returns(c => 
            {
                var sale = c.Arg<Sale>();
                sale.Id = Guid.NewGuid(); // Set the ID when the repository creates the sale
                return sale;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBeEmpty();
        result.Items.Should().HaveCount(command.Items.Count);
        
        // Verify repository calls
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>());
        await _itemRepository.Received(items.Count).GetByIdAsync(Arg.Any<Guid>());
        await _itemRepository.Received(items.Count).UpdateAsync(Arg.Any<Item>());
    }

    [Fact(DisplayName = "Given non-existent item When creating sale Then throws exception")]
    public async Task Handle_NonExistentItem_ThrowsException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        _itemRepository.GetByIdAsync(Arg.Any<Guid>())
            .Returns((Item?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Item with ID {command.Items.First().ItemId} not found");
    }

    [Fact(DisplayName = "Given insufficient stock When creating sale Then throws exception")]
    public async Task Handle_InsufficientStock_ThrowsException()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var item = new Item
        {
            Id = command.Items.First().ItemId,
            Name = "Test Item",
            Price = 10.0m,
            QuantityInStock = 1 // Less than requested quantity
        };

        _itemRepository.GetByIdAsync(item.Id)
            .Returns(item);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"Insufficient stock for item {item.Name}");
    }

    [Fact(DisplayName = "Given valid sale When creating Then calculates correct totals")]
    public async Task Handle_ValidRequest_CalculatesCorrectTotals()
    {
        // Arrange
        var command = new CreateSaleCommand
        {
            CustomerId = Guid.NewGuid(),
            BranchId = Guid.NewGuid(),
            Items = new List<CreateSaleItemCommand>
            {
                new() { ItemId = Guid.NewGuid(), Quantity = 5 },  // Should get 10% discount
                new() { ItemId = Guid.NewGuid(), Quantity = 15 }  // Should get 20% discount
            }
        };

        var items = command.Items.Select(i => new Item
        {
            Id = i.ItemId,
            Name = "Test Item",
            Price = 100.0m,
            QuantityInStock = 20
        }).ToList();

        foreach (var item in items)
        {
            _itemRepository.GetByIdAsync(item.Id)
                .Returns(item);
        }

        _saleRepository.CreateAsync(Arg.Any<Sale>())
            .Returns(c => 
            {
                var sale = c.Arg<Sale>();
                sale.Id = Guid.NewGuid(); // Set the ID when the repository creates the sale
                return sale;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        
        // First item: 5 * 100 = 500, 10% discount = 50
        // Second item: 15 * 100 = 1500, 20% discount = 300
        // Total amount = 2000
        // Total discount = 350
        // Final amount = 1650
        result.TotalAmount.Should().Be(2000);
        result.TotalDiscount.Should().Be(350);
        result.FinalAmount.Should().Be(1650);

        result.Items.Should().HaveCount(2);
        
        var firstItem = result.Items.First(i => i.Quantity == 5);
        firstItem.UnitPrice.Should().Be(100);
        firstItem.DiscountPercentage.Should().Be(10);
        firstItem.DiscountAmount.Should().Be(50);
        firstItem.FinalAmount.Should().Be(450);

        var secondItem = result.Items.First(i => i.Quantity == 15);
        secondItem.UnitPrice.Should().Be(100);
        secondItem.DiscountPercentage.Should().Be(20);
        secondItem.DiscountAmount.Should().Be(300);
        secondItem.FinalAmount.Should().Be(1200);
    }
} 