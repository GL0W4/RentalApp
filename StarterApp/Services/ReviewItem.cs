namespace StarterApp.Services;

public class ReviewItem
{
    public int Id { get; set; }

    public int? RentalId { get; set; }
    public int? ItemId { get; set; }
    public string? ItemTitle { get; set; }

    public int ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;

    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public string RatingDisplay => $"{Rating}/5";

    public string CreatedAtDisplay => CreatedAt.ToString("dd/MM/yyyy");
}