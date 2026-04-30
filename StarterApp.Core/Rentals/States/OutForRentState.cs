namespace StarterApp.Core.Rentals.States;

public sealed class OutForRentState : RentalStateBase
{
    public override string Status => RentalStatuses.OutForRent;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Returned
        };
}