using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync();
    Task<Item> AddItemAsync(CreateItemRequest request);
    Task<List<ItemCategory>> GetCategoriesAsync();
    Task<Item> UpdateItemAsync(int itemId, UpdateItemRequest request);
    Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm, string? category = null);
}