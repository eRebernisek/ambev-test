using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

/// <summary>
/// Repository interface for managing items
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Gets an item by its ID
    /// </summary>
    /// <param name="id">The item ID</param>
    /// <returns>The item if found, null otherwise</returns>
    Task<Item?> GetByIdAsync(Guid id);

    /// <summary>
    /// Updates an item in the system
    /// </summary>
    /// <param name="item">The item to update</param>
    /// <returns>The updated item</returns>
    Task<Item> UpdateAsync(Item item);

    /// <summary>
    /// Creates a new item in the repository
    /// </summary>
    /// <param name="item">The item to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created item</returns>
    Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all items from the repository
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of all items</returns>
    Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an item from the repository
    /// </summary>
    /// <param name="id">The unique identifier of the item to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the item was deleted, false if not found</returns>
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
} 