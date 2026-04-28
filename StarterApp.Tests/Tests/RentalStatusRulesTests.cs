using StarterApp.Core.Rentals;
using Xunit;

namespace StarterApp.Tests;

public class RentalStatusRulesTests
{
    [Fact]
    public void CanApproveOrReject_ReturnsTrue_ForRequested()
    {
        Assert.True(RentalStatusRules.CanApproveOrReject("Requested"));
    }

    [Fact]
    public void CanApproveOrReject_ReturnsFalse_ForApproved()
    {
        Assert.False(RentalStatusRules.CanApproveOrReject("Approved"));
    }

    [Fact]
    public void CanMarkOutForRent_ReturnsTrue_ForApproved()
    {
        Assert.True(RentalStatusRules.CanMarkOutForRent("Approved"));
    }

    [Fact]
    public void CanMarkReturned_ReturnsTrue_ForOutForRent()
    {
        Assert.True(RentalStatusRules.CanMarkReturned("Out for Rent"));
    }

    [Fact]
    public void CanMarkReturned_ReturnsTrue_ForOverdue()
    {
        Assert.True(RentalStatusRules.CanMarkReturned("Overdue"));
    }

    [Fact]
    public void CanComplete_ReturnsTrue_ForReturned()
    {
        Assert.True(RentalStatusRules.CanComplete("Returned"));
    }

    [Theory]
    [InlineData("Approved")]
    [InlineData("Rejected")]
    [InlineData("Out for Rent")]
    [InlineData("Returned")]
    [InlineData("Completed")]
    public void CanApproveOrReject_ReturnsFalse_ForNonRequestedStatuses(string status)
    {
        Assert.False(RentalStatusRules.CanApproveOrReject(status));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Rejected")]
    [InlineData("Out for Rent")]
    [InlineData("Returned")]
    [InlineData("Completed")]
    public void CanMarkOutForRent_ReturnsFalse_ForNonApprovedStatuses(string status)
    {
        Assert.False(RentalStatusRules.CanMarkOutForRent(status));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Approved")]
    [InlineData("Rejected")]
    [InlineData("Returned")]
    [InlineData("Completed")]
    public void CanMarkReturned_ReturnsFalse_ForInvalidStatuses(string status)
    {
        Assert.False(RentalStatusRules.CanMarkReturned(status));
    }

    [Theory]
    [InlineData("Requested")]
    [InlineData("Approved")]
    [InlineData("Rejected")]
    [InlineData("Out for Rent")]
    [InlineData("Completed")]
    public void CanComplete_ReturnsFalse_ForNonReturnedStatuses(string status)
    {
        Assert.False(RentalStatusRules.CanComplete(status));
    }

    [Fact]
    public void RentalStatusRules_HandleNullStatus()
    {
        Assert.False(RentalStatusRules.CanApproveOrReject(null));
        Assert.False(RentalStatusRules.CanMarkOutForRent(null));
        Assert.False(RentalStatusRules.CanMarkReturned(null));
        Assert.False(RentalStatusRules.CanComplete(null));
    }
}
