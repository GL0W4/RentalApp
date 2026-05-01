using StarterApp.Services;

namespace StarterApp.Repositories;

public interface IReviewRepository
{
    Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10);

    Task<ReviewItem> CreateAsync(CreateReviewRequest request, string jwtToken);
}