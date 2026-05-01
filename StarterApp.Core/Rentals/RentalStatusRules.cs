using StarterApp.Core.Rentals.States;

namespace StarterApp.Core.Rentals;

public static class RentalStatusRules
{
    public static bool CanTransition(string? currentStatus, string nextStatus)
    {
        var state = RentalStateFactory.Create(currentStatus);

        return state?.CanTransitionTo(nextStatus) ?? false;
    }
    public static bool CanApproveOrReject(string? status)
    {
        return CanTransition(status, RentalStatuses.Approved) 
        || CanTransition(status, RentalStatuses.Rejected);
    }

    public static bool CanMarkOutForRent(string? status)
    {
        return CanTransition(status, RentalStatuses.OutForRent);
    }

    public static bool CanMarkReturned(string? status)
    {
        return CanTransition(status, RentalStatuses.Returned);
    }

    public static bool CanComplete(string? status)
    {
        return CanTransition(status, RentalStatuses.Completed);
    }

    public static bool CanReview(string? status)
{
    return string.Equals(status, RentalStatuses.Completed, StringComparison.OrdinalIgnoreCase);
}
}