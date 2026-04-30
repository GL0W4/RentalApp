namespace StarterApp.Core.Rentals.States;

public sealed class RequestedState : RentalStateBase
{
    public override string Status => RentalStatuses.Requested;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Approved,
            RentalStatuses.Rejected
        };
}