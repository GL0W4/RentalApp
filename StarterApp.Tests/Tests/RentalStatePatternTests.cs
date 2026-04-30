using StarterApp.Core.Rentals;
using StarterApp.Core.Rentals.States;
using Xunit;

namespace StarterApp.Tests;

public class RentalStatePatternTests
{
    [Theory]
    [InlineData(RentalStatuses.Requested, RentalStatuses.Approved, true)]
    [InlineData(RentalStatuses.Requested, RentalStatuses.Rejected, true)]
    [InlineData(RentalStatuses.Requested, RentalStatuses.OutForRent, false)]
    [InlineData(RentalStatuses.Approved, RentalStatuses.OutForRent, true)]
    [InlineData(RentalStatuses.Approved, RentalStatuses.Rejected, false)]
    [InlineData(RentalStatuses.OutForRent, RentalStatuses.Returned, true)]
    [InlineData(RentalStatuses.OutForRent, RentalStatuses.Completed, false)]
    [InlineData(RentalStatuses.Overdue, RentalStatuses.Returned, true)]
    [InlineData(RentalStatuses.Returned, RentalStatuses.Completed, true)]
    [InlineData(RentalStatuses.Completed, RentalStatuses.Returned, false)]
    [InlineData(RentalStatuses.Rejected, RentalStatuses.Approved, false)]
    public void CanTransition_ReturnsExpectedResult(
        string currentStatus,
        string nextStatus,
        bool expected)
    {
        var result = RentalStatusRules.CanTransition(currentStatus, nextStatus);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_ReturnsNull_ForUnknownStatus()
    {
        var state = RentalStateFactory.Create("Unknown");

        Assert.Null(state);
    }

    [Fact]
    public void Create_ReturnsRequestedState_ForRequestedStatus()
    {
        var state = RentalStateFactory.Create(RentalStatuses.Requested);

        Assert.IsType<RequestedState>(state);
    }

    [Fact]
    public void CompletedState_HasNoAllowedTransitions()
    {
        var state = new CompletedState();

        Assert.Empty(state.AllowedTransitions);
    }

    [Fact]
    public void RejectedState_HasNoAllowedTransitions()
    {
        var state = new RejectedState();

        Assert.Empty(state.AllowedTransitions);
    }
}