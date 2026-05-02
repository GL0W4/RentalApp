namespace StarterApp.Services;

/// <summary>
/// Response DTO containing reviews and aggregate rating details for an item.
/// </summary>
public class ItemReviewsResult
{
    /// <summary>Gets or sets the reviews returned for the requested page.</summary>
    public List<ReviewItem> Reviews { get; set; } = new();

    /// <summary>Gets or sets the average rating for the item, when supplied by the API.</summary>
    public double? AverageRating { get; set; }

    /// <summary>Gets or sets the total number of reviews for the item.</summary>
    public int TotalReviews { get; set; }

    /// <summary>Gets or sets the current result page number.</summary>
    public int Page { get; set; }

    /// <summary>Gets or sets the number of reviews requested per page.</summary>
    public int PageSize { get; set; }

    /// <summary>Gets or sets the total number of available pages.</summary>
    public int TotalPages { get; set; }
}
