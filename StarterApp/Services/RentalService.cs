using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace StarterApp.Services;

public class RentalService : IRentalService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthenticationService _authService;

    public RentalService(IAuthenticationService authService)
    {
        _authService = authService;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev")
        };
    }

    public async Task SubmitRentalRequestAsync(CreateRentalRequest request)
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to request a rental.");
        }

        using var message = new HttpRequestMessage(HttpMethod.Post, "/rentals");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (errorBody.Contains("date", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Please choose valid rental dates.");
            }

            if (errorBody.Contains("conflict", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("overlap", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("available", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("This item is not available for the selected dates.");
            }

            if (errorBody.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("token", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Your session has expired. Please log in again.");
            }

            throw new Exception($"Failed to submit rental request. API said: {errorBody}");
        }
    }

    private async Task<string> GetRequiredTokenAsync()
    {
        var jwtToken = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(jwtToken))
        {
            throw new Exception("You must be logged in to view rental requests.");
        }

        return jwtToken;
    }

    public async Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string? status = null)
    {
        var jwtToken = await GetRequiredTokenAsync();

        var url = "/rentals/incoming";
        if (!string.IsNullOrWhiteSpace(status))
        {
            url += $"?status={Uri.EscapeDataString(status)}";
        }

        using var message = new HttpRequestMessage(HttpMethod.Get, url);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to load incoming rentals. API said: {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<RentalListResponse>();
        return result?.Rentals ?? new List<RentalRequestItem>();
    }

    public async Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string? status = null)
    {
        var jwtToken = await GetRequiredTokenAsync();

        var url = "/rentals/outgoing";
        if (!string.IsNullOrWhiteSpace(status))
        {
            url += $"?status={Uri.EscapeDataString(status)}";
        }

        using var message = new HttpRequestMessage(HttpMethod.Get, url);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to load outgoing rentals. API said: {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<RentalListResponse>();
        return result?.Rentals ?? new List<RentalRequestItem>();
    }
}