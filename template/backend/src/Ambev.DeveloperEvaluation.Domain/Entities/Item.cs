using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item that can be sold in the system.
/// </summary>
public class Item : BaseEntity
{
    /// <summary>
    /// Gets or sets the name of the item.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the price of the item.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the quantity in stock.
    /// </summary>
    public int QuantityInStock { get; set; }

    /// <summary>
    /// Validates the item entity.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new ItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    /// <summary>
    /// Updates the stock quantity after a sale.
    /// </summary>
    /// <param name="quantity">The quantity to reduce from stock.</param>
    /// <returns>True if the stock was successfully updated, false if there's insufficient stock.</returns>
    public bool UpdateStock(int quantity)
    {
        if (QuantityInStock < quantity)
            return false;

        QuantityInStock -= quantity;
        return true;
    }
} 