using StarterApp.Database.Models;
using StarterApp.Repositories;

namespace StarterApp.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    public ItemService(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        return await _itemRepository.GetAllAsync();
    }

    public async Task<Item> AddItemAsync(CreateItemRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to create an item.");
        }

        return await _itemRepository.CreateAsync(request, jwtToken);
    }

    public async Task<List<ItemCategory>> GetCategoriesAsync()
    {
        return await _itemRepository.GetCategoriesAsync();
    }

    public async Task<Item> UpdateItemAsync(int itemId, UpdateItemRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to update an item.");
        }

        return await _itemRepository.UpdateAsync(itemId, request, jwtToken);
    }
}