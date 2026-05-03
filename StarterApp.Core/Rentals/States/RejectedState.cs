namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Terminal state used when an owner declines a requested rental.
/// </summary>
public sealed class RejectedState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Rejected;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        Array.Empty<string>();
}
