using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Sales;

public class CreateSaleTests : IntegrationTest
{
    [Fact]
    public async Task Create_ValidSale_ReturnsCreated()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var customer = await CreateCustomer();
        var branch = await CreateBranch();
        var item = await CreateItem();

        var command = new CreateSaleCommand
        {
            CustomerId = customer.Id,
            BranchId = branch.Id,
            Items = new List<CreateSaleItemCommand>
            {
                new() { ItemId = item.Id, Quantity = 5 }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<CreateSaleResult>();
        result.Should().NotBeNull();
        result!.Id.Should().NotBeEmpty();
        result.Items.Should().HaveCount(1);

        // Verify database state
        var sale = await _dbContext.Set<Sale>()
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == result.Id);
        
        sale.Should().NotBeNull();
        sale!.CustomerId.Should().Be(customer.Id);
        sale.BranchId.Should().Be(branch.Id);
        sale.Items.Should().HaveCount(1);

        var saleItem = sale.Items.First();
        saleItem.ItemId.Should().Be(item.Id);
        saleItem.Quantity.Should().Be(5);
        saleItem.UnitPrice.Should().Be(item.Price);

        // Verify stock was updated
        var updatedItem = await _dbContext.Set<Item>().FindAsync(item.Id);
        updatedItem.Should().NotBeNull();
        updatedItem!.QuantityInStock.Should().Be(item.QuantityInStock - 5);
    }

    [Fact]
    public async Task Create_NonExistentItem_ReturnsNotFound()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var customer = await CreateCustomer();
        var branch = await CreateBranch();

        var command = new CreateSaleCommand
        {
            CustomerId = customer.Id,
            BranchId = branch.Id,
            Items = new List<CreateSaleItemCommand>
            {
                new() { ItemId = Guid.NewGuid(), Quantity = 5 }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_InsufficientStock_ReturnsConflict()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var customer = await CreateCustomer();
        var branch = await CreateBranch();
        var item = await CreateItem(quantityInStock: 3);

        var command = new CreateSaleCommand
        {
            CustomerId = customer.Id,
            BranchId = branch.Id,
            Items = new List<CreateSaleItemCommand>
            {
                new() { ItemId = item.Id, Quantity = 5 }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Create_InvalidData_ReturnsBadRequest()
    {
        // Arrange
        await AuthenticateAsAdmin();

        var command = new CreateSaleCommand
        {
            CustomerId = Guid.Empty,
            BranchId = Guid.Empty,
            Items = new List<CreateSaleItemCommand>()
        };

        // Act
        var response = await _client.PostAsJsonAsync("api/sales", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<Customer> CreateCustomer()
    {
        var customer = new Customer { Name = "Test Customer" };
        _dbContext.Set<Customer>().Add(customer);
        await _dbContext.SaveChangesAsync();
        return customer;
    }

    private async Task<Branch> CreateBranch()
    {
        var branch = new Branch { Name = "Test Branch", Location = "Test Location" };
        _dbContext.Set<Branch>().Add(branch);
        await _dbContext.SaveChangesAsync();
        return branch;
    }

    private async Task<Item> CreateItem(int quantityInStock = 10)
    {
        var item = new Item
        {
            Name = "Test Item",
            Price = 100.0m,
            QuantityInStock = quantityInStock
        };
        _dbContext.Set<Item>().Add(item);
        await _dbContext.SaveChangesAsync();
        return item;
    }
} 