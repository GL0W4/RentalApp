namespace StarterApp.Services;

/// <summary>
/// Review DTO displayed on item detail pages and returned after review submission.
/// </summary>
public class ReviewItem
{
    /// <summary>Gets or sets the review identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the related rental identifier, when returned by the API.</summary>
    public int? RentalId { get; set; }
    /// <summary>Gets or sets the reviewed item identifier, when returned by the API.</summary>
    public int? ItemId { get; set; }
    /// <summary>Gets or sets the reviewed item title, when included in the response.</summary>
    public string? ItemTitle { get; set; }

    /// <summary>Gets or sets the identifier of the user who wrote the review.</summary>
    public int ReviewerId { get; set; }
    /// <summary>Gets or sets the display name of the reviewer.</summary>
    public string ReviewerName { get; set; } = string.Empty;

    /// <summary>Gets or sets the numeric rating value.</summary>
    public int Rating { get; set; }
    /// <summary>Gets or sets the optional review comment.</summary>
    public string? Comment { get; set; }
    /// <summary>Gets or sets the date and time the review was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets the formatted rating shown in the UI.</summary>
    public string RatingDisplay => $"{Rating}/5";

    /// <summary>Gets the formatted review date shown in the UI.</summary>
    public string CreatedAtDisplay => CreatedAt.ToString("dd/MM/yyyy");
}
