using StarterApp.Services;

namespace StarterApp.Repositories;

/// <summary>
/// Defines API operations for reading item reviews and submitting completed-rental reviews.
/// </summary>
public interface IReviewRepository
{
    /// <summary>Gets paged review data and rating summary information for an item.</summary>
    Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10);

    /// <summary>Creates a review using the authenticated borrower's JWT.</summary>
    Task<ReviewItem> CreateAsync(CreateReviewRequest request, string jwtToken);
}
