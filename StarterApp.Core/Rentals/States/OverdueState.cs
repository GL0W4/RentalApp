namespace StarterApp.Core.Rentals.States;

public sealed class OverdueState : RentalStateBase
{
    public override string Status => RentalStatuses.Overdue;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Returned
        };
}