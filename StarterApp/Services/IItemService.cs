using StarterApp.Database.Models;

namespace StarterApp.Services;

/// <summary>
/// Coordinates item workflows between ViewModels and the item API repository.
/// </summary>
public interface IItemService
{
    /// <summary>Gets all item listings currently exposed by the hosted API.</summary>
    Task<List<Item>> GetItemsAsync();
    /// <summary>Creates an item listing for the authenticated user.</summary>
    Task<Item> AddItemAsync(CreateItemRequest request);
    /// <summary>Gets categories that can be assigned to item listings.</summary>
    Task<List<ItemCategory>> GetCategoriesAsync();
    /// <summary>Gets a single item directly from the API by identifier.</summary>
    Task<Item?> GetItemByIdAsync(int itemId);
    /// <summary>Updates an item listing owned by the authenticated user.</summary>
    Task<Item> UpdateItemAsync(int itemId, UpdateItemRequest request);
    /// <summary>Gets item listings near a location with optional category filtering.</summary>
    Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm, string? category = null);
}
