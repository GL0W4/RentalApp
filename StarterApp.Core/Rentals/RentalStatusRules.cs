using StarterApp.Core.Rentals.States;

namespace StarterApp.Core.Rentals;

/// <summary>
/// Provides workflow checks for moving rental requests through their allowed lifecycle.
/// </summary>
public static class RentalStatusRules
{
    /// <summary>
    /// Determines whether a rental may move from its current status to a requested next status.
    /// </summary>
    public static bool CanTransition(string? currentStatus, string nextStatus)
    {
        var state = RentalStateFactory.Create(currentStatus);

        return state?.CanTransitionTo(nextStatus) ?? false;
    }

    /// <summary>
    /// Indicates whether an owner can approve or reject a request in the current state.
    /// </summary>
    public static bool CanApproveOrReject(string? status)
    {
        return CanTransition(status, RentalStatuses.Approved) 
        || CanTransition(status, RentalStatuses.Rejected);
    }

    /// <summary>
    /// Indicates whether an approved rental can be marked as out for rent.
    /// </summary>
    public static bool CanMarkOutForRent(string? status)
    {
        return CanTransition(status, RentalStatuses.OutForRent);
    }

    /// <summary>
    /// Indicates whether a rental in progress can be marked as returned.
    /// </summary>
    public static bool CanMarkReturned(string? status)
    {
        return CanTransition(status, RentalStatuses.Returned);
    }

    /// <summary>
    /// Indicates whether a returned rental can be completed by the owner.
    /// </summary>
    public static bool CanComplete(string? status)
    {
        return CanTransition(status, RentalStatuses.Completed);
    }

    /// <summary>
    /// Reviews are only permitted once a rental has reached the completed state.
    /// </summary>
    public static bool CanReview(string? status)
{
    return string.Equals(status, RentalStatuses.Completed, StringComparison.OrdinalIgnoreCase);
}
}
