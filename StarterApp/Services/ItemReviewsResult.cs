namespace StarterApp.Services;

public class ItemReviewsResult
{
    public List<ReviewItem> Reviews { get; set; } = new();

    public double AverageRating { get; set; }

    public int TotalReviews { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalPages { get; set; }
}