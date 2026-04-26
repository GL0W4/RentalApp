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

    public bool CanOwnerRespond =>
    string.Equals(Status, "Requested", StringComparison.OrdinalIgnoreCase);

    public string StartDateDisplay =>
    DateTime.TryParse(StartDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : StartDate;

    public string EndDateDisplay =>
    DateTime.TryParse(EndDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : EndDate;

    public bool CanApproveOrReject =>
    string.Equals(Status, "Requested", StringComparison.OrdinalIgnoreCase);

    public bool CanMarkOutForRent =>
        string.Equals(Status, "Approved", StringComparison.OrdinalIgnoreCase);

    public bool CanMarkReturned =>
        string.Equals(Status, "Out for Rent", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Status, "Overdue", StringComparison.OrdinalIgnoreCase);

    public bool CanComplete =>
        string.Equals(Status, "Returned", StringComparison.OrdinalIgnoreCase);
}