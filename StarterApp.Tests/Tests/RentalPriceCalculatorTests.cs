using StarterApp.Core.Rentals;
using Xunit;

namespace StarterApp.Tests;

public class RentalPriceCalculatorTests
{
    [Fact]
    public void CalculateRentalDays_ReturnsCorrectDuration()
    {
        var start = new DateTime(2026, 4, 27);
        var end = new DateTime(2026, 4, 30);

        var days = RentalPriceCalculator.CalculateRentalDays(start, end);

        Assert.Equal(3, days);
    }

    [Fact]
    public void CalculateRentalDays_ReturnsZero_WhenEndBeforeStart()
    {
        var start = new DateTime(2026, 4, 30);
        var end = new DateTime(2026, 4, 27);

        var days = RentalPriceCalculator.CalculateRentalDays(start, end);

        Assert.Equal(0, days);
    }

    [Fact]
    public void CalculateRentalDays_ReturnsOne_WhenEndDateIsNextDay()
    {
        var start = new DateTime(2026, 4, 27);
        var end = new DateTime(2026, 4, 28);

        var days = RentalPriceCalculator.CalculateRentalDays(start, end);

        Assert.Equal(1, days);
    }

    [Fact]
    public void CalculateRentalDays_ReturnsZero_WhenDatesAreSame()
    {
        var date = new DateTime(2026, 4, 27);

        var days = RentalPriceCalculator.CalculateRentalDays(date, date);

        Assert.Equal(0, days);
    }

    [Fact]
    public void CalculateEstimatedTotal_ReturnsDailyRateTimesDays()
    {
        var start = new DateTime(2026, 4, 27);
        var end = new DateTime(2026, 4, 30);

        var total = RentalPriceCalculator.CalculateEstimatedTotal(25m, start, end);

        Assert.Equal(75m, total);
    }

    [Fact]
    public void CalculateEstimatedTotal_ReturnsZero_WhenDurationIsZero()
    {
        var date = new DateTime(2026, 4, 27);

        var total = RentalPriceCalculator.CalculateEstimatedTotal(25m, date, date);

        Assert.Equal(0m, total);
    }

    [Fact]
    public void CalculateEstimatedTotal_HandlesDecimalDailyRates()
    {
        var start = new DateTime(2026, 4, 27);
        var end = new DateTime(2026, 4, 29);

        var total = RentalPriceCalculator.CalculateEstimatedTotal(12.50m, start, end);

        Assert.Equal(25.00m, total);
    }
}