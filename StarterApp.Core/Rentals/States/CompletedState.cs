namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Final successful rental state; completed rentals may be reviewed.
/// </summary>
public sealed class CompletedState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Completed;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        Array.Empty<string>();
}
