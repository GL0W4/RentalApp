namespace StarterApp.Core.Rentals;

/// <summary>
/// Contains client-side overdue checks used to display rental warnings in the UI.
/// </summary>
public static class RentalOverdueRules
{
    /// <summary>
    /// Returns true when an active rental has passed its agreed end date.
    /// </summary>
    public static bool IsOverdue(string? status, DateTime endDate, DateTime today)
    {
        return string.Equals(status, RentalStatuses.OutForRent, StringComparison.OrdinalIgnoreCase)
            && endDate.Date < today.Date;
    }
}
