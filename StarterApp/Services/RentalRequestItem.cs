using StarterApp.Core.Rentals;

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

    public string StartDateDisplay =>
    DateTime.TryParse(StartDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : StartDate;

    public string EndDateDisplay =>
    DateTime.TryParse(EndDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : EndDate;

    public bool CanApproveOrReject => RentalStatusRules.CanApproveOrReject(Status);

    public bool CanMarkOutForRent => RentalStatusRules.CanMarkOutForRent(Status);

    public bool CanMarkReturned => RentalStatusRules.CanMarkReturned(Status);

    public bool CanComplete => RentalStatusRules.CanComplete(Status);

    public bool IsLocallyOverdue
    {
        get
        {
            return DateTime.TryParse(EndDate, out var endDate)
                && RentalOverdueRules.IsOverdue(Status, endDate, DateTime.Today);
        }
    }

    public string StatusDisplay =>
        IsLocallyOverdue ? RentalStatuses.Overdue : Status;

    public string? OverdueWarning =>
        IsLocallyOverdue ? "This rental is past its end date." : null;
}
