using StarterApp.Database.Models;

namespace StarterApp.Services;

public interface IItemService
{
    Task<List<Item>> GetItemsAsync();
}