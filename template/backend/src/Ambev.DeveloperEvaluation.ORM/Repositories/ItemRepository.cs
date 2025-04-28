using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of IItemRepository using Entity Framework Core
/// </summary>
public class ItemRepository : IItemRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of ItemRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public ItemRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new item in the database
    /// </summary>
    /// <param name="item">The item to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created item</returns>
    public async Task<Item> CreateAsync(Item item, CancellationToken cancellationToken = default)
    {
        await _context.Items.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return item;
    }

    /// <summary>
    /// Updates an existing item in the database
    /// </summary>
    /// <param name="item">The item to update</param>
    /// <returns>The updated item</returns>
    public async Task<Item> UpdateAsync(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

    /// <summary>
    /// Retrieves an item by its unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the item</param>
    /// <returns>The item if found, null otherwise</returns>
    public async Task<Item?> GetByIdAsync(Guid id)
    {
        return await _context.Items.FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <summary>
    /// Retrieves all items from the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A list of all items</returns>
    public async Task<IEnumerable<Item>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Items.ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes an item from the database
    /// </summary>
    /// <param name="id">The unique identifier of the item to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the item was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(id);
        if (item == null)
            return false;

        _context.Items.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
} 