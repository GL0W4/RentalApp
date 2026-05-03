namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Rental state used after the owner accepts the request but before handover.
/// </summary>
public sealed class ApprovedState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Approved;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.OutForRent
        };
}
