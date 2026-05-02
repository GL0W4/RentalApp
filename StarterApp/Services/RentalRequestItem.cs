using StarterApp.Core.Rentals;

namespace StarterApp.Services;

/// <summary>
/// Rental request DTO used by the incoming and outgoing rental request screens.
/// </summary>
public class RentalRequestItem
{
    /// <summary>Gets or sets the rental request identifier.</summary>
    public int Id { get; set; }
    /// <summary>Gets or sets the related item identifier.</summary>
    public int ItemId { get; set; }
    /// <summary>Gets or sets the related item title.</summary>
    public string ItemTitle { get; set; } = string.Empty;

    /// <summary>Gets or sets the borrower user identifier.</summary>
    public int? BorrowerId { get; set; }
    /// <summary>Gets or sets the borrower display name.</summary>
    public string? BorrowerName { get; set; }
    /// <summary>Gets or sets the borrower average rating, when supplied by the API.</summary>
    public double? BorrowerRating { get; set; }

    /// <summary>Gets or sets the item owner user identifier.</summary>
    public int? OwnerId { get; set; }
    /// <summary>Gets or sets the item owner display name.</summary>
    public string? OwnerName { get; set; }
    /// <summary>Gets or sets the owner average rating, when supplied by the API.</summary>
    public double? OwnerRating { get; set; }

    /// <summary>Gets or sets the rental start date as returned by the API.</summary>
    public string StartDate { get; set; } = string.Empty;
    /// <summary>Gets or sets the rental end date as returned by the API.</summary>
    public string EndDate { get; set; } = string.Empty;
    /// <summary>Gets or sets the current workflow status.</summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>Gets or sets the total rental price calculated by the API.</summary>
    public decimal TotalPrice { get; set; }

    /// <summary>Gets or sets when the rental was requested.</summary>
    public DateTime? RequestedAt { get; set; }
    /// <summary>Gets or sets when the rental was approved.</summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>Gets the start date formatted for display in the UI.</summary>
    public string StartDateDisplay =>
    DateTime.TryParse(StartDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : StartDate;

    /// <summary>Gets the end date formatted for display in the UI.</summary>
    public string EndDateDisplay =>
    DateTime.TryParse(EndDate, out var date)
        ? date.ToString("dd/MM/yyyy")
        : EndDate;

    /// <summary>Gets whether the request can currently be approved or rejected.</summary>
    public bool CanApproveOrReject => RentalStatusRules.CanApproveOrReject(Status);

    /// <summary>Gets whether the rental can be marked as handed over.</summary>
    public bool CanMarkOutForRent => RentalStatusRules.CanMarkOutForRent(Status);

    /// <summary>Gets whether the borrower can mark the rental as returned.</summary>
    public bool CanMarkReturned => RentalStatusRules.CanMarkReturned(Status);

    /// <summary>Gets whether the owner can complete the returned rental.</summary>
    public bool CanComplete => RentalStatusRules.CanComplete(Status);

    /// <summary>Gets whether the borrower can leave a review for the rental.</summary>
    public bool CanReview => RentalStatusRules.CanReview(Status);

    /// <summary>
    /// Gets whether the client should display the rental as overdue based on today's date.
    /// </summary>
    public bool IsLocallyOverdue
    {
        get
        {
            return DateTime.TryParse(EndDate, out var endDate)
                && RentalOverdueRules.IsOverdue(Status, endDate, DateTime.Today);
        }
    }

    /// <summary>Gets the status text displayed in the UI, including local overdue detection.</summary>
    public string StatusDisplay =>
        IsLocallyOverdue ? RentalStatuses.Overdue : Status;

    /// <summary>Gets the warning text shown when a rental is locally overdue.</summary>
    public string? OverdueWarning =>
        IsLocallyOverdue ? "This rental is past its end date." : null;
}
