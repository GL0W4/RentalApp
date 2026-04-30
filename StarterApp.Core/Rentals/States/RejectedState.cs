namespace StarterApp.Core.Rentals.States;

public sealed class RejectedState : RentalStateBase
{
    public override string Status => RentalStatuses.Rejected;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        Array.Empty<string>();
}