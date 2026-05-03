using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.Repositories;

/// <summary>
/// Defines API operations for browsing, creating, updating, and locating rental items.
/// </summary>
public interface IItemRepository : IRepository<Item>
{
    /// <summary>Creates a new item listing using the authenticated user's JWT.</summary>
    Task<Item> CreateAsync(CreateItemRequest request, string jwtToken);

    /// <summary>Updates an existing item listing using the authenticated user's JWT.</summary>
    Task<Item> UpdateAsync(int itemId, UpdateItemRequest request, string jwtToken);

    /// <summary>Retrieves the item categories supplied by the hosted API.</summary>
    Task<List<ItemCategory>> GetCategoriesAsync();

    /// <summary>Retrieves detailed information for a single item by its API identifier.</summary>
    Task<Item?> GetByIdAsync(int itemId);

    /// <summary>Retrieves items near the supplied coordinates, optionally constrained by category.</summary>
    Task<List<Item>> GetNearbyAsync(double latitude, double longitude, double radiusKm, string? category = null);
}
