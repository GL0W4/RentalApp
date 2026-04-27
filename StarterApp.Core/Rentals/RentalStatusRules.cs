namespace StarterApp.Core.Rentals;

public static class RentalStatusRules
{
    public static bool CanApproveOrReject(string? status)
    {
        return string.Equals(status, RentalStatuses.Requested, StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanMarkOutForRent(string? status)
    {
        return string.Equals(status, RentalStatuses.Approved, StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanMarkReturned(string? status)
    {
        return string.Equals(status, RentalStatuses.OutForRent, StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, RentalStatuses.Overdue, StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanComplete(string? status)
    {
        return string.Equals(status, RentalStatuses.Returned, StringComparison.OrdinalIgnoreCase);
    }
}