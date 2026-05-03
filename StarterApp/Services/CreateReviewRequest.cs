namespace StarterApp.Services;

/// <summary>
/// Request body sent to the API when a borrower reviews a completed rental.
/// </summary>
public class CreateReviewRequest
{
    /// <summary>Gets or sets the completed rental being reviewed.</summary>
    public int RentalId { get; set; }

    /// <summary>Gets or sets the numeric rating value.</summary>
    public int Rating { get; set; }

    /// <summary>Gets or sets the optional review comment.</summary>
    public string? Comment { get; set; }
}
