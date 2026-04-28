using StarterApp.Core.Items;
using Xunit;

namespace StarterApp.Tests;

public class ItemValidationRulesTests
{
    [Fact]
    public void HasTitle_ReturnsFalse_ForNullOrWhitespace()
    {
        Assert.False(ItemValidationRules.HasTitle(null));
        Assert.False(ItemValidationRules.HasTitle(""));
        Assert.False(ItemValidationRules.HasTitle("   "));
    }

    [Fact]
    public void HasTitle_ReturnsTrue_ForNonEmptyTitle()
    {
        Assert.True(ItemValidationRules.HasTitle("Power Drill"));
    }

    [Fact]
    public void HasValidTitleLength_ReturnsFalse_WhenTitleTooShort()
    {
        Assert.False(ItemValidationRules.HasValidTitleLength("Abcd"));
    }

    [Fact]
    public void HasValidTitleLength_ReturnsTrue_WhenTitleMeetsMinimum()
    {
        Assert.True(ItemValidationRules.HasValidTitleLength("Drill"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void IsValidDailyRate_ReturnsFalse_ForZeroOrNegative(decimal dailyRate)
    {
        Assert.False(ItemValidationRules.IsValidDailyRate(dailyRate));
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(25)]
    [InlineData(1000)]
    public void IsValidDailyRate_ReturnsTrue_ForPositiveValues(decimal dailyRate)
    {
        Assert.True(ItemValidationRules.IsValidDailyRate(dailyRate));
    }

    [Theory]
    [InlineData(-90)]
    [InlineData(0)]
    [InlineData(90)]
    public void IsValidLatitude_ReturnsTrue_ForBoundaryValues(double latitude)
    {
        Assert.True(ItemValidationRules.IsValidLatitude(latitude));
    }

    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    public void IsValidLatitude_ReturnsFalse_WhenOutsideRange(double latitude)
    {
        Assert.False(ItemValidationRules.IsValidLatitude(latitude));
    }

    [Theory]
    [InlineData(-180)]
    [InlineData(0)]
    [InlineData(180)]
    public void IsValidLongitude_ReturnsTrue_ForBoundaryValues(double longitude)
    {
        Assert.True(ItemValidationRules.IsValidLongitude(longitude));
    }

    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    public void IsValidLongitude_ReturnsFalse_WhenOutsideRange(double longitude)
    {
        Assert.False(ItemValidationRules.IsValidLongitude(longitude));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(50.1)]
    public void IsValidSearchRadius_ReturnsFalse_WhenOutsideAllowedRange(double radiusKm)
    {
        Assert.False(ItemValidationRules.IsValidSearchRadius(radiusKm));
    }

    [Theory]
    [InlineData(0.1)]
    [InlineData(5)]
    [InlineData(50)]
    public void IsValidSearchRadius_ReturnsTrue_WhenWithinAllowedRange(double radiusKm)
    {
        Assert.True(ItemValidationRules.IsValidSearchRadius(radiusKm));
    }
}