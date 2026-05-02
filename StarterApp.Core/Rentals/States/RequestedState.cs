namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Rental state used immediately after a borrower submits a request.
/// </summary>
public sealed class RequestedState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Requested;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Approved,
            RentalStatuses.Rejected
        };
}
