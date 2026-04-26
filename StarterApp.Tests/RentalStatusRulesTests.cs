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
}