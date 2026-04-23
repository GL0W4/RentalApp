using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync();
    Task<Item> AddItemAsync(CreateItemRequest request);
    Task<List<ItemCategory>> GetCategoriesAsync();
}