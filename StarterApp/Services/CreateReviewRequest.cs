namespace StarterApp.Services;

public class CreateReviewRequest
{
    public int RentalId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }
}