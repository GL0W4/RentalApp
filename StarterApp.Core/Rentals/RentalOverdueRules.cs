namespace StarterApp.Core.Rentals;

public static class RentalOverdueRules
{
    public static bool IsOverdue(string? status, DateTime endDate, DateTime today)
    {
        return string.Equals(status, RentalStatuses.OutForRent, StringComparison.OrdinalIgnoreCase)
            && endDate.Date < today.Date;
    }
}