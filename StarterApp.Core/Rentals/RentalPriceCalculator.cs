namespace StarterApp.Core.Rentals;

/// <summary>
/// Calculates simple rental date and price values for request previews.
/// </summary>
public static class RentalPriceCalculator
{
    /// <summary>
    /// Calculates the number of chargeable days between the selected start and end dates.
    /// </summary>
    public static int CalculateRentalDays(DateTime startDate, DateTime endDate)
    {
        return Math.Max(0, (endDate.Date - startDate.Date).Days);
    }

    /// <summary>
    /// Calculates the estimated total price before the request is submitted to the API.
    /// </summary>
    public static decimal CalculateEstimatedTotal(decimal dailyRate, DateTime startDate, DateTime endDate)
    {
        var days = CalculateRentalDays(startDate, endDate);
        return dailyRate * days;
    }
}
