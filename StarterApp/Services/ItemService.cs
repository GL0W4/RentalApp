using System.Net.Http.Json;
using StarterApp.Database.Models;

namespace StarterApp.Services;

public class ItemService : IItemService
{
    private readonly HttpClient _httpClient;

    public ItemService()
    {
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

    private class ItemListResponse
    {
        public List<Item> Items { get; set; } = new();
    }
}