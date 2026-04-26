using System.Net.Http.Headers;
using System.Net.Http.Json;
using StarterApp.Services;

namespace StarterApp.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly HttpClient _httpClient;

    public RentalRepository()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev")
        };
    }

    public async Task SubmitRentalRequestAsync(CreateRentalRequest request, string jwtToken)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/rentals");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
               throw new Exception("This item is already booked for the selected dates.");
            }

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

            if (errorBody.Contains("own item", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("You cannot rent your own item.");
            }

            if (errorBody.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
                errorBody.Contains("token", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Your session has expired. Please log in again.");
            }

            throw new Exception("Failed to submit rental request.");
        }
    }

    public async Task<List<RentalRequestItem>> GetIncomingRentalsAsync(string jwtToken, string? status = null)
    {
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
            throw new Exception("Failed to load incoming rental requests.");
        }

        var result = await response.Content.ReadFromJsonAsync<RentalListResponse>();
        return result?.Rentals ?? new List<RentalRequestItem>();
    }

    public async Task<List<RentalRequestItem>> GetOutgoingRentalsAsync(string jwtToken, string? status = null)
    {
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
            throw new Exception("Failed to load outgoing rental requests.");
        }

        var result = await response.Content.ReadFromJsonAsync<RentalListResponse>();
        return result?.Rentals ?? new List<RentalRequestItem>();
    }

    public async Task UpdateRentalStatusAsync(int rentalId, string status, string jwtToken)
    {
        var request = new UpdateRentalStatusRequest
        {
            Status = status
        };

        using var message = new HttpRequestMessage(HttpMethod.Patch, $"/rentals/{rentalId}/status");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("You can only update rental requests you are involved in.");
            }

            if (errorBody.Contains("Invalid state transition", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("This rental request cannot be updated to the selected status.");
            }

            if (errorBody.Contains("owner", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Only the owner of this item can approve or reject rental requests.");
            }

            throw new Exception($"Failed to update rental status: {(int)response.StatusCode} {response.ReasonPhrase}. API said: {errorBody}");
        }
    }
}