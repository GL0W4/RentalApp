using StarterApp.Database.Models;
using StarterApp.Repositories;
using StarterApp.Core.Items;

namespace StarterApp.Services;

/// <summary>
/// Provides item-related business checks before delegating persistence to the API repository.
/// </summary>
public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    /// <summary>
    /// Creates an item service with API repository and authentication dependencies.
    /// </summary>
    public ItemService(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
    }

    /// <inheritdoc />
    public async Task<List<Item>> GetItemsAsync()
    {
        return await _itemRepository.GetAllAsync();
    }

    /// <inheritdoc />
    public async Task<Item> AddItemAsync(CreateItemRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to create an item.");
        }

        return await _itemRepository.CreateAsync(request, jwtToken);
    }

    /// <inheritdoc />
    public async Task<List<ItemCategory>> GetCategoriesAsync()
    {
        return await _itemRepository.GetCategoriesAsync();
    }

    /// <inheritdoc />
    public async Task<Item?> GetItemByIdAsync(int itemId)
    {
        if (itemId <= 0)
        {
            throw new ArgumentException("Item ID must be valid.", nameof(itemId));
        }

        return await _itemRepository.GetByIdAsync(itemId);
    }

    /// <inheritdoc />
    public async Task<Item> UpdateItemAsync(int itemId, UpdateItemRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to update an item.");
        }

        return await _itemRepository.UpdateAsync(itemId, request, jwtToken);
    }

    /// <inheritdoc />
    public async Task<List<Item>> GetNearbyItemsAsync(double latitude, double longitude, double radiusKm, string? category = null)
    {
        if (!ItemValidationRules.IsValidLatitude(latitude))
        {
            throw new Exception("Latitude must be between -90 and 90.");
        }

        if (!ItemValidationRules.IsValidLongitude(longitude))
        {
            throw new Exception("Longitude must be between -180 and 180.");
        }

        if (!ItemValidationRules.IsValidSearchRadius(radiusKm))
        {
            throw new Exception("Radius must be between 1 and 50 km.");
        }

        return await _itemRepository.GetNearbyAsync(latitude, longitude, radiusKm, category);
    }
}
