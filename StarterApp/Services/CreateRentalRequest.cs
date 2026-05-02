namespace StarterApp.Services;

/// <summary>
/// Request body sent to the API when a borrower requests an item rental.
/// </summary>
public class CreateRentalRequest
{
    /// <summary>Gets or sets the item identifier being requested.</summary>
    public int ItemId { get; set; }
    /// <summary>Gets or sets the requested start date in API format.</summary>
    public string StartDate { get; set; } = string.Empty;
    /// <summary>Gets or sets the requested end date in API format.</summary>
    public string EndDate { get; set; } = string.Empty;
}
