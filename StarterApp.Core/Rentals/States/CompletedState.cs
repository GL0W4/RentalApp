namespace StarterApp.Core.Rentals.States;

public sealed class CompletedState : RentalStateBase
{
    public override string Status => RentalStatuses.Completed;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        Array.Empty<string>();
}