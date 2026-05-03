namespace StarterApp.Core.Rentals.States;

/// <summary>
/// State used when the rental period has passed before the item is returned.
/// </summary>
public sealed class OverdueState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Overdue;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Returned
        };
}
