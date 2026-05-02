namespace StarterApp.Services;

/// <summary>
/// Coordinates item review retrieval and review submission validation.
/// </summary>
public interface IReviewService
{
    /// <summary>Gets reviews and aggregate rating information for an item.</summary>
    Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10);

    /// <summary>Submits a review for a completed rental.</summary>
    Task<ReviewItem> CreateReviewAsync(int rentalId, int rating, string? comment);
}
