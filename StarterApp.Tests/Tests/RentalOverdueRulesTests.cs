using StarterApp.Core.Rentals;

namespace StarterApp.Tests.Tests;

public class RentalOverdueRulesTests
{
    [Fact]
    public void IsOverdue_ReturnsTrue_WhenOutForRentAndEndDateBeforeToday()
    {
        var today = new DateTime(2026, 4, 30);
        var endDate = new DateTime(2026, 4, 29);

        var result = RentalOverdueRules.IsOverdue(
            RentalStatuses.OutForRent,
            endDate,
            today);

        Assert.True(result);
    }

    [Fact]
    public void IsOverdue_ReturnsFalse_WhenOutForRentAndEndDateIsToday()
    {
        var today = new DateTime(2026, 4, 30);

        var result = RentalOverdueRules.IsOverdue(
            RentalStatuses.OutForRent,
            today,
            today);

        Assert.False(result);
    }

    [Fact]
    public void IsOverdue_ReturnsFalse_WhenOutForRentAndEndDateAfterToday()
    {
        var today = new DateTime(2026, 4, 30);
        var endDate = new DateTime(2026, 5, 1);

        var result = RentalOverdueRules.IsOverdue(
            RentalStatuses.OutForRent,
            endDate,
            today);

        Assert.False(result);
    }

    [Theory]
    [InlineData(RentalStatuses.Requested)]
    [InlineData(RentalStatuses.Approved)]
    [InlineData(RentalStatuses.Returned)]
    [InlineData(RentalStatuses.Completed)]
    [InlineData(RentalStatuses.Rejected)]
    public void IsOverdue_ReturnsFalse_WhenStatusIsNotOutForRent(string status)
    {
        var today = new DateTime(2026, 4, 30);
        var endDate = new DateTime(2026, 4, 29);

        var result = RentalOverdueRules.IsOverdue(status, endDate, today);

        Assert.False(result);
    }

    [Fact]
    public void IsOverdue_IsCaseInsensitive_ForOutForRentStatus()
    {
        var today = new DateTime(2026, 4, 30);
        var endDate = new DateTime(2026, 4, 29);

        var result = RentalOverdueRules.IsOverdue(
            "out for rent",
            endDate,
            today);

        Assert.True(result);
    }
}