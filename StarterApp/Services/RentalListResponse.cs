namespace StarterApp.Services;

/// <summary>
/// Response DTO returned by the API for incoming and outgoing rental lists.
/// </summary>
public class RentalListResponse
{
    /// <summary>Gets or sets the rental request records in the response.</summary>
    public List<RentalRequestItem> Rentals { get; set; } = new();
    /// <summary>Gets or sets the total number of rentals returned by the API.</summary>
    public int TotalRentals { get; set; }
}
