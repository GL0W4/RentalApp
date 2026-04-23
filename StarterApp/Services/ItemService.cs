using System.Net.Http.Headers;
using System.Net.Http.Json;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ItemService : IItemService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authService;

    public ItemService(IAuthenticationService authService)
    {
        _authService = authService;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev")
        };
    }

    public async Task<List<Item>> GetItemsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ItemListResponse>("/items");
        return response?.Items ?? new List<Item>();
    }

    public async Task<Item> AddItemAsync(CreateItemRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to create an item.");
        }

        using var message = new HttpRequestMessage(HttpMethod.Post, "/items");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            
            if (errorBody.Contains("category", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Invalid category selected. Please choose a valid category.");
            }

            if (errorBody.Contains("latitude", StringComparison.OrdinalIgnoreCase) || 
                errorBody.Contains("longitude", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Invalid location coordinates. Please ensure latitude is between -90 and 90, and longitude is between -180 and 180.");
            }

            if (errorBody.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) || 
                errorBody.Contains("token", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Your session has expired. Please log in again.");
            }

            throw new Exception("Failed to create item.");
        }

        var createdItem = await response.Content.ReadFromJsonAsync<Item>();

        if (createdItem == null)
        {
            throw new Exception("Create item failed: empty response.");
        }

        return createdItem;
    }

    private class ItemListResponse
    {
        public List<Item> Items { get; set; } = new();
    }

    public async Task<List<ItemCategory>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<CategoryListResponse>("/categories");
        return response?.Categories ?? new List<ItemCategory>();
    }

    private class CategoryListResponse
    {
        public List<ItemCategory> Categories { get; set; } = new();
    }
}