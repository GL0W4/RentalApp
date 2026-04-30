namespace StarterApp.Core.Rentals.States;

public sealed class ApprovedState : RentalStateBase
{
    public override string Status => RentalStatuses.Approved;

    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.OutForRent
        };
}