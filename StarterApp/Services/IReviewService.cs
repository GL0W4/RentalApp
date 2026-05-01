namespace StarterApp.Services;

public interface IReviewService
{
    Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10);

    Task<ReviewItem> CreateReviewAsync(int rentalId, int rating, string? comment);
}