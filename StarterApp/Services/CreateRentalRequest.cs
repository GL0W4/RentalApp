namespace StarterApp.Services;

public class CreateRentalRequest
{
    public int ItemId { get; set; }
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
}