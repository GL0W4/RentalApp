namespace StarterApp.Core.Rentals;

public static class RentalStatusRules
{
    public static bool CanApproveOrReject(string? status)
    {
        return string.Equals(status, "Requested", StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanMarkOutForRent(string? status)
    {
        return string.Equals(status, "Approved", StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanMarkReturned(string? status)
    {
        return string.Equals(status, "Out for Rent", StringComparison.OrdinalIgnoreCase)
            || string.Equals(status, "Overdue", StringComparison.OrdinalIgnoreCase);
    }

    public static bool CanComplete(string? status)
    {
        return string.Equals(status, "Returned", StringComparison.OrdinalIgnoreCase);
    }
}