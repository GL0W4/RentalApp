namespace StarterApp.Core.Rentals.States;

/// <summary>
/// Rental state used after the borrower indicates that the item has been returned.
/// </summary>
public sealed class ReturnedState : RentalStateBase
{
    /// <inheritdoc />
    public override string Status => RentalStatuses.Returned;

    /// <inheritdoc />
    public override IReadOnlyCollection<string> AllowedTransitions =>
        new[]
        {
            RentalStatuses.Completed
        };
}
