namespace StarterApp.Services;

/// <summary>
/// Request body sent to the API when creating a new item listing.
/// </summary>
public class CreateItemRequest
{
    /// <summary>Gets or sets the listing title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the optional listing description.</summary>
    public string? Description { get; set; }
    /// <summary>Gets or sets the daily rental price.</summary>
    public decimal DailyRate { get; set; }
    /// <summary>Gets or sets the selected API category identifier.</summary>
    public int CategoryId { get; set; }
    /// <summary>Gets or sets the latitude used for nearby item discovery.</summary>
    public double Latitude { get; set; }
    /// <summary>Gets or sets the longitude used for nearby item discovery.</summary>
    public double Longitude { get; set; }
}
