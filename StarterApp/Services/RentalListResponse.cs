namespace StarterApp.Services;

public class RentalListResponse
{
    public List<RentalRequestItem> Rentals { get; set; } = new();
    public int TotalRentals { get; set; }
}