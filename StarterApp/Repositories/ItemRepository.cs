using System.Net.Http.Headers;
using System.Net.Http.Json;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly HttpClient _httpClient;

    public ItemRepository()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.BaseUrl)
        };
    }

    public async Task<List<Item>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ItemListResponse>("/items");
        return response?.Items ?? new List<Item>();
    }

    public async Task<Item> CreateAsync(CreateItemRequest request, string jwtToken)
    {
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

    public async Task<Item> UpdateAsync(int itemId, UpdateItemRequest request, string jwtToken)
    {
        using var message = new HttpRequestMessage(HttpMethod.Put, $"/items/{itemId}");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("You can only update items that you own.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Item not found.");
            }

            if (errorBody.Contains("title", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Title must be at least 5 characters long.");
            }

            if (errorBody.Contains("dailyRate", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Daily rate must be valid.");
            }

            throw new Exception("Failed to update item.");
        }

        var updatedItem = await response.Content.ReadFromJsonAsync<Item>();

        if (updatedItem == null)
        {
            throw new Exception("Update item failed: empty response.");
        }

        return updatedItem;
    }

    public async Task<List<ItemCategory>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<CategoryListResponse>("/categories");
        return response?.Categories ?? new List<ItemCategory>();
    }

    private class ItemListResponse
    {
        public List<Item> Items { get; set; } = new();
    }

    private class CategoryListResponse
    {
        public List<ItemCategory> Categories { get; set; } = new();
    }

    public async Task<List<Item>> GetNearbyAsync(double latitude, double longitude, double radiusKm, string? category = null)
    {
        var url = $"/items/nearby?lat={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                  $"&lon={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                  $"&radius={radiusKm.ToString(System.Globalization.CultureInfo.InvariantCulture)}";

        if (!string.IsNullOrWhiteSpace(category))
        {
            url += $"&category={Uri.EscapeDataString(category)}";
        }

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (errorBody.Contains("coordinates", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("latitude", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("longitude", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Invalid location coordinates.");
            }

            throw new Exception($"Failed to load nearby items. API said: {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<NearbyItemListResponse>();
        return result?.Items ?? new List<Item>();
    }

    private class NearbyItemListResponse
    {
        public List<Item> Items { get; set; } = new();
        public LocationResult? SearchLocation { get; set; }
        public double Radius { get; set; }
        public int TotalResults { get; set; }
    }
}