namespace StarterApp.Services;

/// <summary>
/// Request body sent to the API when updating an existing item listing.
/// </summary>
public class UpdateItemRequest
{
    /// <summary>Gets or sets the updated listing title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the updated optional listing description.</summary>
    public string? Description { get; set; }
    /// <summary>Gets or sets the updated daily rental price.</summary>
    public decimal DailyRate { get; set; }
    /// <summary>Gets or sets whether the item is currently available for rental.</summary>
    public bool IsAvailable { get; set; }
}
