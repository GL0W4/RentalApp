namespace StarterApp.Core.Rentals;

public static class RentalPriceCalculator
{
    public static int CalculateRentalDays(DateTime startDate, DateTime endDate)
    {
        return Math.Max(0, (endDate.Date - startDate.Date).Days);
    }

    public static decimal CalculateEstimatedTotal(decimal dailyRate, DateTime startDate, DateTime endDate)
    {
        var days = CalculateRentalDays(startDate, endDate);
        return dailyRate * days;
    }
}