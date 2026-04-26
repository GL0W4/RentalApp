using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.Repositories;

public interface IItemRepository : IRepository<Item>
{
    Task<Item> CreateAsync(CreateItemRequest request, string jwtToken);
    Task<Item> UpdateAsync(int itemId, UpdateItemRequest request, string jwtToken);
    Task<List<ItemCategory>> GetCategoriesAsync();
}