using System.Net.Http.Headers;
using System.Net.Http.Json;
using StarterApp.Services;

namespace StarterApp.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly HttpClient _httpClient;

    public ReviewRepository()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConstants.BaseUrl)
        };
    }

    public async Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10)
    {
        var response = await _httpClient.GetAsync($"/items/{itemId}/reviews?page={page}&pageSize={pageSize}");

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new Exception("Item not found.");
            }

            throw new Exception($"Failed to load reviews. API said: {errorBody}");
        }

        var json = await response.Content.ReadAsStringAsync();
    

         var result = System.Text.Json.JsonSerializer.Deserialize<ItemReviewsResult>(
        json,
        new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null)
        {
            throw new Exception("Load reviews failed: empty response.");
        }

        return result;
    }

    public async Task<ReviewItem> CreateAsync(CreateReviewRequest request, string jwtToken)
    {
        using var message = new HttpRequestMessage(HttpMethod.Post, "/reviews");
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        message.Content = JsonContent.Create(request);

        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new Exception("Your session has expired. Please log in again.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new Exception("You can only review rentals where you were the borrower.");
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                throw new Exception("You have already reviewed this rental.");
            }

            if (errorBody.Contains("rating", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Rating must be between 1 and 5.");
            }

            if (errorBody.Contains("completed", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("You can only review completed rentals.");
            }

            throw new Exception($"Failed to submit review. API said: {errorBody}");
        }

        var createdReview = await response.Content.ReadFromJsonAsync<ReviewItem>();

        if (createdReview == null)
        {
            throw new Exception("Submit review failed: empty response.");
        }

        return createdReview;
    }
}