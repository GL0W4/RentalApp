namespace StarterApp.Core.Rentals.States;

public sealed class ReturnedState : RentalStateBase
{
    public override string Status => RentalStatuses.Returned;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Completed
        };
}