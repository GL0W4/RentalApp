namespace StarterApp.Services;

public class RentalRequestItem
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string ItemTitle { get; set; } = string.Empty;

    public int? BorrowerId { get; set; }
    public string? BorrowerName { get; set; }
    public double? BorrowerRating { get; set; }

    public int? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public double? OwnerRating { get; set; }

    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    public DateTime? RequestedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
}