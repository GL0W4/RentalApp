using StarterApp.Core.Reviews;
using StarterApp.Repositories;

namespace StarterApp.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IAuthenticationService _authService;

    public ReviewService(IReviewRepository reviewRepository, IAuthenticationService authService)
    {
        _reviewRepository = reviewRepository;
        _authService = authService;
    }

    public async Task<ItemReviewsResult> GetItemReviewsAsync(int itemId, int page = 1, int pageSize = 10)
    {
        if (itemId <= 0)
        {
            throw new ArgumentException("Item ID must be valid.", nameof(itemId));
        }

        return await _reviewRepository.GetItemReviewsAsync(itemId, page, pageSize);
    }

    public async Task<ReviewItem> CreateReviewAsync(int rentalId, int rating, string? comment)
    {
        if (rentalId <= 0)
        {
            throw new ArgumentException("Rental ID must be valid.", nameof(rentalId));
        }

        if (!ReviewValidationRules.IsValidRating(rating))
        {
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));
        }

        if (!ReviewValidationRules.IsValidComment(comment))
        {
            throw new ArgumentException("Comment must be 500 characters or fewer.", nameof(comment));
        }

        var token = await _authService.GetValidTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("You must be logged in to submit a review.");
        }

        var request = new CreateReviewRequest
        {
            RentalId = rentalId,
            Rating = rating,
            Comment = comment
        };

        return await _reviewRepository.CreateAsync(request, token);
    }
}