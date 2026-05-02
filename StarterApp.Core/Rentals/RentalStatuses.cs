namespace StarterApp.Core.Rentals;

/// <summary>
/// Defines the canonical rental status values used by the app and hosted API.
/// </summary>
public static class RentalStatuses
{
    /// <summary>Initial state created when a borrower submits a rental request.</summary>
    public const string Requested = "Requested";
    /// <summary>Owner-approved state before the item is handed over.</summary>
    public const string Approved = "Approved";
    /// <summary>Terminal state for requests declined by the item owner.</summary>
    public const string Rejected = "Rejected";
    /// <summary>State used while the borrower currently has the item.</summary>
    public const string OutForRent = "Out for Rent";
    /// <summary>Derived or API state indicating that the rental end date has passed.</summary>
    public const string Overdue = "Overdue";
    /// <summary>State used after the borrower marks the item as returned.</summary>
    public const string Returned = "Returned";
    /// <summary>Final successful rental state that enables review submission.</summary>
    public const string Completed = "Completed";
}
